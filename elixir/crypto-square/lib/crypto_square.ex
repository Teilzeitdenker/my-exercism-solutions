defmodule CryptoSquare do
  def encode(""), do: ""
  def encode(str) do
    normalized = str |> String.replace(~r/\W/, "") |> String.downcase
    c = normalized |> String.length |> :math.sqrt() |> ceil()
    normalized |> String.to_charlist() |> Enum.chunk_every(c, c, ' ' |> Stream.cycle |> Enum.take(c)) |> transpose()
  end
  defp transpose(chars), do: chars |> Enum.zip |> Enum.map(&Tuple.to_list/1) |> Enum.map(&to_string/1) |> Enum.join(" ")
end
