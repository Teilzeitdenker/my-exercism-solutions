defmodule Series do
  @doc """
  Finds the largest product of a given number of consecutive numbers in a given string of numbers.
  """
  @spec largest_product(String.t(), non_neg_integer) :: non_neg_integer
  def largest_product(s, span) do
    cond do
      span == 0 -> 1 # empty product is 1, regardless of parsing success!
      span < 0 or span > String.length(s) or Integer.parse(s) == :error -> raise ArgumentError
      {n, rest} = Integer.parse(s) ->
        cond do
          rest != "" -> raise ArgumentError # some non-digits were hidden inside
          n == 0 -> 0 # otherwise Enum.chunk_every() would raise
          true ->
            n
            |> Integer.digits
            |> Enum.chunk_every(span, 1, :discard)
            |> Enum.map(&Enum.reduce(&1, 1, fn el, acc -> acc * el end))
            |> Enum.max()
        end
    end
  end
end
