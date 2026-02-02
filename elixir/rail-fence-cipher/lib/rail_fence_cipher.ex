defmodule RailFenceCipher do
  @doc """
  Encode a given plaintext to the corresponding rail fence ciphertext
  """
  @spec encode(String.t(), pos_integer) :: String.t()
  def encode(str, rails) do
    up_and_down(rails)
    |> Enum.zip(str |> String.codepoints)
    |> Enum.group_by(&elem(&1, 0), &elem(&1, 1))
    |> Map.values
    |> Enum.map(&to_string/1)
    |> Enum.join
  end

  @doc """
  Decode a given rail fence ciphertext to the corresponding plaintext
  """
  @spec decode(String.t(), pos_integer) :: String.t()
  def decode(str, rails) do
    up_and_down(rails)
    |> Enum.zip(Enum.to_list(1..(str |> String.length)))
    |> Enum.sort
    |> Enum.map(&elem(&1,1))
    |> Enum.zip(str |> String.codepoints)
    |> Enum.sort
    |> Enum.map(&elem(&1,1))
    |> to_string
  end

  defp up_and_down(rails) do
    if rails == 1 do
      Stream.cycle([1])
    else
      Stream.cycle(Enum.to_list(1..rails-1) ++ Enum.to_list(rails..2//-1))
    end
  end
end
