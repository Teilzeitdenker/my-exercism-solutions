defmodule RunLengthEncoder do
  @doc """
  Generates a string where consecutive elements are represented as a data value and count.
  "AABBBCCCC" => "2A3B4C"
  For this example, assume all input are strings, that are all uppercase letters.
  It should also be able to reconstruct the data into its original form.
  "2A3B4C" => "AABBBCCCC"
  """
  @spec encode(String.t()) :: String.t()
  def encode(input) do
    input
    |> String.codepoints
    |> Enum.chunk_by(&(&1))
    |> Enum.map(fn ls -> if length(ls) > 1, do: to_string(length(ls)) <> Enum.at(ls, 0), else: Enum.at(ls, 0) end)
    |> Enum.join("")
  end

  @spec decode(String.t()) :: String.t()
  def decode(input) do
    re = ~r/(\d+)(\D)/
    Regex.replace(re, input, fn _, num, char -> String.duplicate(char, String.to_integer(num)) end)
  end
end
