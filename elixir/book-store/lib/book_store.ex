defmodule BookStore do
  @typedoc "A book is represented by its number in the 5-book series"
  @type book :: 1 | 2 | 3 | 4 | 5
  @prices [3000, 2560, 2160, 1520, 800]

  @doc """
  Calculate lowest price (in cents) for a shopping basket containing books.
  """
  @spec total(basket :: [book]) :: integer
  def total(basket) do
    sorted_counts = [1, 2, 3, 4, 5] |> Enum.map(fn i -> Enum.count(basket, &(&1 == i)) end) |> Enum.sort()
    stack_counts = sorted_counts |> Enum.zip([0] ++ (sorted_counts |> Enum.map(&-/1))) |> Enum.map(&Tuple.sum/1) # numbers of stacks with 5, 4, 3, 2, 1 different books from the series
    pairs_of_3_and_5 = min(stack_counts |> Enum.at(0), stack_counts |> Enum.at(2)) # any pair of these has to be converted into two 4-stacks to get the minimal price
    optimal_stack_counts =
      [Enum.at(stack_counts, 0) - pairs_of_3_and_5,
      Enum.at(stack_counts, 1) + 2 * pairs_of_3_and_5,
      Enum.at(stack_counts, 2) - pairs_of_3_and_5,
      Enum.at(stack_counts, 3), Enum.at(stack_counts, 4)]
    optimal_stack_counts |> Enum.zip(@prices) |> Enum.map(&Tuple.product/1) |> Enum.sum()
  end
end
