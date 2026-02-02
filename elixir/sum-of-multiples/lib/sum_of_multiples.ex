defmodule SumOfMultiples do
  @doc """
  Adds up all numbers from 1 to a given end number that are multiples of the factors provided.
  """
  @spec to(non_neg_integer, [non_neg_integer]) :: non_neg_integer
  def to(_limit, []), do: 0
  def to(_limit, [0]), do: 0
  def to(limit, factors) do
    factors
    |> Enum.filter(fn n -> n > 0 end)
    |> Enum.flat_map(fn n -> Enum.to_list(n..(limit-1)//n) end)
    |> Enum.uniq()
    |> Enum.sum()
  end
end
