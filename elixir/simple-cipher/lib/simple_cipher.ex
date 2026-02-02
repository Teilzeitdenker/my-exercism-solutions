defmodule SimpleCipher do
  @doc """
  Given a `plaintext` and `key`, encode each character of the `plaintext` by
  shifting it by the corresponding letter in the alphabet shifted by the number
  of letters represented by the `key` character, repeating the `key` if it is
  shorter than the `plaintext`.

  For example, for the letter 'd', the alphabet is rotated to become:

  defghijklmnopqrstuvwxyzabc

  You would encode the `plaintext` by taking the current letter and mapping it
  to the letter in the same position in this rotated alphabet.

  abcdefghijklmnopqrstuvwxyz
  defghijklmnopqrstuvwxyzabc

  "a" becomes "d", "t" becomes "w", etc...

  Each letter in the `plaintext` will be encoded with the alphabet of the `key`
  character in the same position. If the `key` is shorter than the `plaintext`,
  repeat the `key`.

  Example:

  plaintext = "testing"
  key = "abc"

  The key should repeat to become the same length as the text, becoming
  "abcabca". If the key is longer than the text, only use as many letters of it
  as are necessary.
  """
  @spec encode(String.t(), String.t()) :: String.t()
  def encode(plaintext, key) do
    plaintext
    |> String.to_charlist
    |> Enum.zip(
        key
        |> String.duplicate(
            ((plaintext |> String.length)
            / (key |> String.length))
            |> ceil
            )
        |> to_charlist)
    |> Enum.map(&encode_char/1)
    |> to_string
  end

  @spec encode_char({integer(), integer()}) :: integer()
  defp encode_char({t, s}) do
    rem(t + s - 2* ?a, 26) + ?a
  end
  @doc """
  Given a `ciphertext` and `key`, decode each character of the `ciphertext` by
  finding the corresponding letter in the alphabet shifted by the number of
  letters represented by the `key` character, repeating the `key` if it is
  shorter than the `ciphertext`.

  The same rules for key length and shifted alphabets apply as in `encode/2`,
  but you will go the opposite way, so "d" becomes "a", "w" becomes "t",
  etc..., depending on how much you shift the alphabet.
  """
  @spec decode(String.t(), String.t()) :: String.t()
  def decode(ciphertext, key) do
    ciphertext
    |> String.to_charlist
    |> Enum.zip(
        key
        |> String.duplicate(
            ((ciphertext |> String.length)
            / (key |> String.length))
            |> ceil
            )
        |> to_charlist)
    |> Enum.map(&decode_char/1)
    |> to_string
  end

  @spec decode_char({integer(), integer()}) :: integer()
  defp decode_char({t, s}) do
    rem(t - (s - 26), 26) + ?a
  end
  @doc """
  Generate a random key of a given length. It should contain lowercase letters only.
  """
  @spec generate_key(integer()) :: String.t()
  def generate_key(length) do
    1..length |> Enum.map(fn _ -> Enum.random(?a..?z) end) |> to_string()
  end
end
