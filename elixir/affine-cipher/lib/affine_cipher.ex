defmodule AffineCipher do
  @typedoc """
  A type for the encryption key
  """
  @type key() :: %{a: integer, b: integer}
  @cands %{1 => 1, 3 => 9, 5 => 21, 7 => 15, 9 => 3, 11 => 19, 15 => 7, 17 => 23, 19 => 11, 21 => 5, 23 => 17, 25 => 25}

  defp valid_key?(a) do
    @cands |> Map.keys |> Enum.member?(a)
  end

  @doc """
  Encode an encrypted message using a key
  """
  @spec encode(key :: key(), message :: String.t()) :: {:ok, String.t()} | {:error, String.t()}
  def encode(%{a: a, b: b}, message) do
    if valid_key?(a) do
      {:ok,
      clean_charlist(message)
        |> Enum.map(fn x -> if digit?(x), do: x, else: affine_encode(x, a, b) end)
        |> Enum.chunk_every(5)
        |> Enum.map(&to_string/1)
        |> Enum.join(" ") }
    else
      {:error, "a and m must be coprime."}
    end
  end

  @doc """
  Decode an encrypted message using a key
  """
  @spec decode(key :: key(), message :: String.t()) :: {:ok, String.t()} | {:error, String.t()}
  def decode(%{a: a, b: b}, encrypted) do
    if valid_key?(a) do
      {:ok,
      clean_charlist(encrypted)
        |> Enum.map(fn y -> if digit?(y), do: y, else: affine_decode(y, a, b) end)
        |> to_string() }
    else
      {:error, "a and m must be coprime."}
    end
  end

  defp digit?(c) do
    ?0..?9 |> Enum.member?(c)
  end

  defp letter_or_digit?(c) do
    ?a..?z |> Enum.member?(c) || digit?(c)
  end

  defp clean_charlist(s) do
    s |> String.downcase() |> String.to_charlist() |> Enum.filter(&letter_or_digit?/1)
  end

  defp codepoint_to_int(c) do
    c - ?a
  end

  defp int_to_codepoint(i) do
    if i < 0 do
      int_to_codepoint(i + 26)
    else
      rem(i, 26) + ?a
    end
  end

  defp affine_encode(x, a, b) do
    int_to_codepoint(a * codepoint_to_int(x) + b)
  end

  defp affine_decode(y, a, b) do
    mod_inv = @cands |> Map.get(a)
    int_to_codepoint(mod_inv * (codepoint_to_int(y) - b))
  end
end
