defmodule Scrabble do
  @spec to_points(String.t()) :: non_neg_integer()
  def to_points(char) do
    case char do
      c when c in ["A", "E", "I", "O", "U", "L", "N", "R", "S", "T"] -> 1
      c when c in ["D", "G"]                                         -> 2
      c when c in ["B", "C", "M", "P"]                               -> 3
      c when c in ["F", "H", "V", "W", "Y"]                          -> 4
      c when c == "K" -> 5
      c when c in ["J", "X"] -> 8
      _ -> 10
    end
  end
  @doc """
  Calculate the scrabble score for the word.
  """
  @spec score(String.t()) :: non_neg_integer
  def score(word) do
    word = Regex.replace(~r/[^[:alpha:]]/, word, "")
    word |> String.upcase() |> String.graphemes() |> Enum.map(&to_points/1) |> Enum.sum()
  end
end
