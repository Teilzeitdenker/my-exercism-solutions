defmodule Transpose do
  def transpose(""), do: ""
  def transpose(input) do
    lines = input |> String.split("\n")
    max_len = lines |> Enum.map(&String.length/1) |> Enum.max()
    lines
    |> Enum.map(&String.pad_trailing(&1, max_len, "*"))
    |> Enum.map(&String.to_charlist/1)
    |> Enum.zip()
    |> Enum.map(&Tuple.to_list/1)
    |> Enum.map(&to_string/1)
    |> Enum.map(&String.trim_trailing(&1, "*"))
    |> Enum.map(&String.replace(&1, ~r/\*/, " "))
    |> Enum.join("\n")
  end
end
