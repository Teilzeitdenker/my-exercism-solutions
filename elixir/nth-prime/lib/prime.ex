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


defmodule PairingHeap do

  if Application.get_env(:priority_queue, :native) do
    @compile :native
    @compile {:hipe, [:o3]}
  end

    @type key :: any
    @type value :: any

    @type t :: {key, value, list} | nil
    @type element :: {key, value}

    @spec delete_min(t) :: t
    def delete_min(nil), do: nil
    def delete_min({_key, _v, sub_heaps}) do
      pair(sub_heaps)
    end

    @spec empty?(t) :: boolean
    def empty?(nil), do: true
    def empty?(_), do: false

    @spec meld(t, t) :: t
    def meld(nil, heap), do: heap
    def meld(heap, nil), do: heap
    def meld(l = {key_l, value_l, sub_l}, r = {key_r, value_r, sub_r}) do
      cond do
        key_l < key_r -> {key_l, value_l, [r | sub_l]}
        true          -> {key_r, value_r, [l | sub_r]}
      end
    end

    @spec merge(t, t) :: t
    def merge(h1, h2), do: meld(h1, h2)

    @spec min(t, element) :: element
    def min(heap, default \\ {nil, nil})
    def min(nil, default), do: default
    def min({key, value, _}, _default), do: {key, value}

    @spec new :: t
    @spec new(key, value) :: t
    def new(), do: nil
    def new(key, value), do: {key, value, []}

    @spec pair([t]) :: t
    defp pair([]), do: nil
    defp pair([h]), do: h
    defp pair([h0, h1 | hs]), do: meld(meld(h0, h1), pair(hs))

    @spec pop(t, element) :: {element, t}
    def pop(heap, default \\ {nil, nil}) do
      {__MODULE__.min(heap, default), delete_min(heap)}
    end

    @spec put(t, key, value) :: t
    def put(heap, key, value) do
      meld(heap, new(key, value))
    end

  end

  defmodule PriorityQueue do

    if Application.get_env(:priority_queue, :native) do
      @compile :native
      @compile {:hipe, [:o3]}
    end


      @type key :: any
      @type value :: any
      @type element :: {key, value}

      @heap PairingHeap


      @type t :: %__MODULE__{size: non_neg_integer, heap: term}
      defstruct size: 0, heap: nil

      @spec delete_min(t) :: t
      def delete_min(pq = %__MODULE__{size: n, heap: heap}) do
        case empty?(pq) do
          true -> pq
          _    -> %{pq | size: n - 1, heap: @heap.delete_min(heap)}
        end
      end

      @spec delete_min!(t) :: t | no_return
      def delete_min!(pq) do
        case empty?(pq) do
          true -> raise PriorityQueue.EmptyError
          _    -> delete_min(pq)
        end
      end

      @spec empty?(t) :: boolean
      def empty?(%__MODULE__{size: 0, heap: nil}), do: true
      def empty?(_), do: false

      @spec merge(t, t) :: t
      def merge(pq = %__MODULE__{size: m, heap: heap0}, %__MODULE__{size: n, heap: heap1}) do
        %{pq | size: m + n, heap: @heap.meld(heap0, heap1)}
      end

      @spec min(t, element) :: element
      def min(%__MODULE__{heap: heap}, default \\ {nil, nil}), do: @heap.min(heap, default)

      @spec min!(t) :: element | no_return
      def min!(pq) do
        case empty?(pq) do
          true -> raise PriorityQueue.EmptyError
          _    -> __MODULE__.min(pq)
        end
      end

      def new, do: %__MODULE__{}

      @spec pop(t, element) :: {element, t}
      def pop(pq = %__MODULE__{size: n, heap: heap}, default \\ {nil, nil}) do
        case empty?(pq) do
          true -> {default, pq}
          _    -> {e, heap} = @heap.pop(heap, default)
                  {e, %{pq | size: n - 1, heap: heap}}
        end
      end

      @spec pop!(t) :: {element, t} | no_return
      def pop!(pq) do
        case empty?(pq) do
          true -> raise PriorityQueue.EmptyError
          _    -> pop(pq)
        end
      end

      @spec put(t, {key, value}) :: t
      @spec put(t, key, value | none) :: t
      def put(pq = %__MODULE__{ size: n, heap: heap }, {key, value}) do
        %{pq | size: n + 1, heap: @heap.put(heap, key, value)}
      end
      def put(pq = %__MODULE__{ size: n, heap: heap }, key, value \\ nil) do
        %{pq | size: n + 1, heap: @heap.put(heap, key, value)}
      end

      @spec size(t) :: non_neg_integer
      def size(%__MODULE__{size: n}), do: n

      @spec to_list(t) :: list
      def to_list(%__MODULE__{size: 0, heap: nil}), do: []
      def to_list(pq) do
        [__MODULE__.min(pq) | to_list(delete_min(pq))]
      end

      @spec keys(t) :: list
      def keys(%__MODULE__{size: 0, heap: nil}), do: []
      def keys(pq) do
        {key, _value} = min(pq)
        [key | keys(delete_min(pq))]
      end

      @spec values(t) :: list
      def values(%__MODULE__{size: 0, heap: nil}), do: []
      def values(pq) do
        {_k, v} = min(pq)
        [v | values(delete_min(pq))]
      end

    end


    defimpl Collectable, for: PriorityQueue do

      def empty(_pq) do
        PriorityQueue.new
      end

      def into(original) do
        {original, fn
          pq, {:cont, {k, v}} -> PriorityQueue.put(pq, k, v)
          pq, {:cont, {k}} -> PriorityQueue.put(pq, k, nil)
          pq, {:cont, k} -> PriorityQueue.put(pq, k, nil)
          pq, :done -> pq
          _, :halt -> :ok
        end}
      end
    end


    defimpl Enumerable, for: PriorityQueue do

      def reduce(_,   {:halt, acc}, _fun),    do: {:halted, acc}
      def reduce(pq,  {:suspend, acc}, fun),  do: {:suspended, acc, &reduce(pq, &1, fun)}
      def reduce(pq,  {:cont, acc}, fun)      do
        cond do
          PriorityQueue.empty?(pq) -> {:done, acc}
          true                     -> {e, pq} = PriorityQueue.pop(pq);
                                      reduce(pq, fun.(e, acc), fun)
        end
      end

      def member?(pq, e = {k, _v}) do
        if PriorityQueue.empty?(pq) do
          {:ok, false}
        else
          {e_h = {k_h, _}, pq} = PriorityQueue.pop(pq)
          cond do
            k_h > k   -> {:ok, false}
            e === e_h -> {:ok, true}
            true      -> member?(pq, e)
          end
        end
      end

      def count(pq), do: {:ok, PriorityQueue.size(pq)}
    end

    defmodule PriorityQueue.EmptyError do
      defexception []

      def message(_) do
        "queue empty error"
      end
    end
