defmodule Yacht do
  @type category ::
          :ones
          | :twos
          | :threes
          | :fours
          | :fives
          | :sixes
          | :full_house
          | :four_of_a_kind
          | :little_straight
          | :big_straight
          | :choice
          | :yacht

  @doc """
  Calculate the score of 5 dice using the given category's scoring method.
  """
  @spec score(category :: category(), dice :: [integer]) :: integer
  def score(category, dice) do
    case category do
      :choice          -> Enum.sum(dice)
      :ones            -> Enum.count(dice, &(&1 == 1)) * 1
      :twos            -> Enum.count(dice, &(&1 == 2)) * 2
      :threes          -> Enum.count(dice, &(&1 == 3)) * 3
      :fours           -> Enum.count(dice, &(&1 == 4)) * 4
      :fives           -> Enum.count(dice, &(&1 == 5)) * 5
      :sixes           -> Enum.count(dice, &(&1 == 6)) * 6
      :yacht           -> if dice |> Enum.uniq |> Enum.count == 1, do: 50, else: 0
      :little_straight -> if Enum.sort(dice) == [1, 2, 3, 4, 5], do: 30, else: 0
      :big_straight    -> if Enum.sort(dice) == [2, 3, 4, 5, 6], do: 30, else: 0
      :full_house      -> if Enum.frequencies(dice) |> Map.values |> Enum.sort == [2, 3], do: Enum.sum(dice), else: 0
      :four_of_a_kind  -> Enum.frequencies(dice) |> Enum.map(fn {k, v} -> if v in [4, 5], do: k * 4, else: 0 end) |> Enum.sum
    end
  end
end
