defmodule Prime do
  defstruct primes: nil, prime_q: nil, candidate: 9, wheel_primes: [2,3,5,7], wheel: nil

  defp insert_prime(pq, p, wheel) do
    wheel = Math.Wheel.update_multiplier(wheel, p)
    PriorityQueue.put(pq, p*p, wheel)
  end

  defp adjust_table(pq, n) do
    case PriorityQueue.min(pq) do
      {min, v} when (min < n) -> pq
                                 |> PriorityQueue.delete_min
                                 |> PriorityQueue.put( Math.Wheel.spin_wheel(v) )
                                 |> adjust_table(n)
      {_min, _v} -> pq
      # :error   -> pq
    end
  end

  defp sieve_next(pq, wheel) do
    {x, wheel} = Math.Wheel.spin_wheel(wheel);
    pq = adjust_table(pq, x)
    case PriorityQueue.min(pq) do
      {min, _v} when (min == x) -> sieve_next(pq, wheel)
      {_min, _v}                 -> {x, {insert_prime(pq, x, wheel), wheel}}
      # :error                    -> {x, {insert_prime(pq, x, wheel), wheel}}
    end
  end
  @doc """
  Produce an endless stream of primes
  ## Examples
      iex> Math.Primes.sieve |> Enum.take(5)
      [2, 3, 5, 7, 11]
  """
  def sieve do
    pq = PriorityQueue.new
    wheel = Math.Wheel.new_wheel

    # first candidate must be prime, initialises our data structures
    {x, wheel} = Math.Wheel.spin_wheel(wheel);
    pq = insert_prime(pq, x, wheel)

    Stream.concat(Math.Wheel.initial_primes ++ [x],
                  Stream.unfold({pq, wheel}, fn {pq, wheel} -> sieve_next(pq, wheel) end) )
  end

  @doc """
  Produce a list of all primes up to (including) "up_to"
  FIXME: Can produce primes greater then requested
  if "up_to" is less than first element of the wheel
  ## Examples
      iex> Math.Primes.sieve(11)
      [2, 3, 5, 7, 11]
  """
  def sieve(up_to) do
    pq = PriorityQueue.new
    wheel = Math.Wheel.new_wheel

    # first candidate must be prime, initialises our data structures
    {x, wheel} = Math.Wheel.spin_wheel(wheel);
    pq = insert_prime(pq, x, wheel)

    # OK, lets get sieving
    sieve(up_to, pq, wheel, [x | Enum.reverse(Math.Wheel.initial_primes)] )
  end


  defp sieve(up_to, pq, wheel, acc) do
    {p, {pq, wheel}} = sieve_next(pq, wheel)
    cond do
      p <= up_to -> sieve(up_to, pq, wheel, [p|acc])
      true      -> Enum.reverse(acc)
    end
  end
  @doc """
  Generates the nth prime.
  """
  @spec nth(non_neg_integer) :: non_neg_integer
  def nth(count) when count > 0 do
    sieve() |> Enum.take(count) |> List.last()
  end
end

defmodule Math.Wheel do

  @moduledoc """
  Wheel which excludes multiples of 2,3,5,7 from the input.
  Significantly faster to store in a tuple than a Struct
  """
  alias Math.Wheel, as: Wheel


  @cycle2357 [10,2,4,2,4,6,2,6,4,2,4,6,6,2,6,4,2,6,4,6,8,4,2,4,2,4,8,6,4,6,2,4,6,2,6,6,4,2,4,6,2,6,4,2,4,2,10,2]


  @cycle @cycle2357
  @cycle_primes [2,3,5,7]


  @doc "Create our data structure to store our wheel"
  def new_wheel(candidate \\ 1, multiplier \\ 1), do: {candidate, multiplier, []}

  @doc """
  Returns a stream to generate numbers in sequence
      iex> Math.Wheel.spin_wheel |> Enum.take(5)
      [11, 13, 17, 19, 23]
  """
  def spin_wheel do
    Stream.unfold(new_wheel(), &spin_wheel/1 )
  end

  @doc """
  Generate next number in sequence (and updated wheel state)
      iex> Math.Wheel.new_wheel |> Math.Wheel.spin_wheel |> elem(0)
      11
  """
  def spin_wheel({i, mult, []}), do: spin_wheel({i, mult, @cycle})
  def spin_wheel({i, 1, [inc | cycle]}), do: {(i+inc), {i+inc, 1, cycle}}
  def spin_wheel({i, mult, [inc | cycle]}), do: {(i+inc)*mult, {i+inc, mult, cycle}}

  @doc """
  Allows the wheel output to be scaled up by a fixed multiplier
      iex> Math.Wheel.new_wheel |> Math.Wheel.update_multiplier(5) |> Math.Wheel.spin_wheel |> elem(0)
      55
  """
  def update_multiplier({i, _mult, cycle}, new_mult), do: {i, new_mult, cycle}

  @doc """
  Accessor for primes used to generate our wheel
      iex> Math.Wheel.initial_primes
      [2, 3, 5, 7]
  """
  def initial_primes, do: @cycle_primes
end


defmodule Math.Wheel.Struct do

  @compile :native
  @compile {:hipe, [:o3]}

  @moduledoc """
  Wheel which excludes multiples of 2,3,5,7 from the input.
  """
  alias Math.Wheel.Struct, as: Wheel

  defstruct candidate: 1, multiplier: 1, cycle: []

  @cycle2357 [10,2,4,2,4,6,2,6,4,2,4,6,6,2,6,4,2,6,4,6,8,4,2,4,2,4,8,6,4,6,2,4,6,2,6,6,4,2,4,6,2,6,4,2,4,2,10,2]

  @cycle @cycle2357


  def new_wheel(candidate \\ 1, multiplier \\ 1), do: %Wheel{candidate: candidate, multiplier: multiplier}

  def spin_wheel do
    Stream.unfold(%Wheel{multiplier: 1}, &spin_wheel/1 )
  end
  def spin_wheel(wheel = %Wheel{cycle: []}), do: spin_wheel(%{wheel | cycle: @cycle})
  def spin_wheel(wheel = %Wheel{candidate: i, multiplier: 1, cycle: [inc | cycle]}) do
    {i+inc, %{wheel | candidate: (i+inc), cycle: cycle}}
  end
  def spin_wheel(wheel = %Wheel{candidate: i, multiplier: m, cycle: [inc | cycle]}) do
    {(i+inc)*m, %{wheel | candidate: (i+inc), cycle: cycle}}
  end

  def update_multiplier(wheel = %Wheel{}, new_mult) do
    %{wheel | multiplier: new_mult}
  end

end
