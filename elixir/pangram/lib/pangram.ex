defmodule Pangram do
  @doc """
  Determines if a word or sentence is a pangram.
  A pangram is a sentence using every letter of the alphabet at least once.

  Returns a boolean.

    ## Examples

      iex> Pangram.pangram?("the quick brown fox jumps over the lazy dog")
      true

  """
  @spec pangram?(String.t()) :: boolean
  def pangram?(sentence) do
    sentence |> String.downcase() |> String.to_charlist() |> Enum.filter(&letter?/1) |> Enum.frequencies() |> Enum.count() == 26
  end

  @spec letter?(char) :: boolean
  defp letter?(ch) do
    ch <= ?z && ch >= ?a
  end
end
