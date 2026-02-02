defmodule BookStore do
  @typedoc "A book is represented by its number in the 5-book series"
  @type book :: 1 | 2 | 3 | 4 | 5

  @doc """
  Calculate lowest price (in cents) for a shopping basket containing books.
  """
  @spec total(basket :: [book]) :: integer
  def total(basket) do
    sorted_counts = [1, 2, 3, 4, 5] |> Enum.map(fn i -> basket |> Enum.count(fn el -> el == i end) end) |> Enum.sort()
    fives = (sorted_counts |> Enum.at(0))
    fours = (sorted_counts |> Enum.at(1)) - (sorted_counts |> Enum.at(0))
    threes = (sorted_counts |> Enum.at(2)) - (sorted_counts |> Enum.at(1))
    twos = (sorted_counts |> Enum.at(3)) - (sorted_counts |> Enum.at(2))
    ones = (sorted_counts |> Enum.at(4)) - (sorted_counts |> Enum.at(3))
    pairs_of_3_and_5 = min(threes, fives)
    ones * 800 +
      twos * 1_520 +
      (threes - pairs_of_3_and_5) * 2_160 +
      (fours + 2 * pairs_of_3_and_5) * 2_560 +
      (fives - pairs_of_3_and_5) * 3_000
  end
end
