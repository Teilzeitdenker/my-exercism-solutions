defmodule Scrabble do
  @spec to_points(char()) :: non_neg_integer()
  defp to_points(char) do
    case char do
      c when c in 'AEIOULNRST' -> 1
      c when c in 'DG'         -> 2
      c when c in 'BCMP'       -> 3
      c when c in 'FHVWY'      -> 4
      c when c in 'K'          -> 5
      c when c in 'JX'         -> 8
      _                        -> 10
    end
  end
  @doc """
  Calculate the scrabble score for the word.
  """
  @spec score(String.t()) :: non_neg_integer
  def score(word) do
    word = Regex.replace(~r/[^[:alpha:]]/, word, "")
    # word |> String.upcase() |> String.graphemes() |> Enum.map(&to_points/1) |> Enum.sum()
    word |> String.upcase() |> String.to_charlist() |> Enum.map(&to_points/1) |> Enum.sum()
  end
end
