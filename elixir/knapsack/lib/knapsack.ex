defmodule Knapsack do
  @doc """
  Return the maximum value that a knapsack can carry.
  """
  @spec maximum_value(items :: [%{value: integer, weight: integer}], maximum_weight :: integer) :: integer
  def maximum_value(items, maximum_weight) do
    items
      |> Enum.reduce(List.duplicate(0, maximum_weight + 1), &process_item/2)
      |> Enum.at(maximum_weight)
  end

  @spec process_item(item :: %{value: integer, weight: integer}, ls :: [integer]) :: [integer]
  defp process_item(item, ls) do
    zipped = (ls |> Enum.drop(item.weight) |> Enum.zip(ls))
    (ls |> Enum.take(item.weight)) ++ (zipped |> Enum.map(fn {upper, lower} -> max(lower + item.value, upper) end))
  end
end
