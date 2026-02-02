defmodule CryptoSquare do
  @doc """
  Encode string square methods
  ## Examples

    iex> CryptoSquare.encode("abcd")
    "ac bd"
  """
  @spec encode(String.t()) :: String.t()
  def encode(""), do: ""
  def encode(str) do
    normalized =
      str
      |> String.replace(~r/[^[:alnum:]]/, "")
      |> String.downcase()
    c = normalized
      |> String.length()
      |> :math.sqrt()
      |> :math.ceil()
      |> Kernel.trunc()
    normalized
      |> String.to_charlist()
      |> Enum.chunk_every(c)
      |> Enum.map(&to_string/1)
      |> Enum.map(&String.pad_trailing(&1, c))
      |> Enum.map(&String.to_charlist/1)
      |> Enum.zip()
      |> Enum.map(&Tuple.to_list/1)
      |> Enum.map(&to_string/1)
      |> Enum.join(" ")
  end
end
