defmodule Transpose do
  @doc """
  Given an input text, output it transposed.

  Rows become columns and columns become rows. See https://en.wikipedia.org/wiki/Transpose.

  If the input has rows of different lengths, this is to be solved as follows:
    * Pad to the left with spaces.
    * Don't pad to the right.

  ## Examples

    iex> Transpose.transpose("ABC\\nDE")
    "AD\\nBE\\nC"

    iex> Transpose.transpose("AB\\nDEF")
    "AD\\nBE\\n F"
  """

  @spec transpose(String.t()) :: String.t()
  def transpose(""), do: ""
  def transpose(input) do
    lines = input |> String.split("\n")
    max_len = lines |> Enum.map(&String.length/1) |> Enum.max()
    lines
    |> Enum.map(fn line ->
      line |> String.pad_trailing(max_len, "*")
    end)
    |> Enum.map(&String.to_charlist/1)
    |> transpose_chars()
    |> Enum.map(&to_string/1)
    |> Enum.map(fn line ->
      Regex.replace(~r/\*/, line |> String.trim_trailing("*"), " ")
    end)
    |> Enum.join("\n")
  end

  @spec transpose_chars([char()]) :: [char()]
  defp transpose_chars(ls) do
    ls |> Enum.zip() |> Enum.map(&Tuple.to_list/1)
  end

end
