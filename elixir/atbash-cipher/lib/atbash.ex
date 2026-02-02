defmodule Atbash do
  @cipher_map for l <- 0..25, into: %{}, do: {<<l + ?a>>, <<25 - l + ?a>>}
  @digits ["1", "2", "3", "4", "5", "6", "7", "8", "9", "0"]
  @doc """
  Encode a given plaintext to the corresponding ciphertext

  ## Examples

  iex> Atbash.encode("completely insecure")
  "xlnko vgvob rmhvx fiv"
  """
  @spec encode(String.t()) :: String.t()
  def encode(plaintext) do
    plaintext
    |> String.replace(~r/[^[:alnum:]]/, "")
    |> String.downcase()
    |> String.codepoints()
    |> Enum.map(fn char -> if char in @digits, do: char, else: @cipher_map[char] end)
    |> Enum.chunk_every(5)
    |> Enum.map(&to_string/1)
    |> Enum.join(" ")
  end

  @spec decode(String.t()) :: String.t()
  def decode(cipher) do
    cipher
    |> String.replace(~r/\s/, "")
    |> String.codepoints()
    |> Enum.map(fn char -> if char in @digits, do: char, else: @cipher_map[char] end)
    |> to_string()
  end
end
