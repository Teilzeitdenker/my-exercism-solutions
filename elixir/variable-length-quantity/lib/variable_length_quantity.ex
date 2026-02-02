defmodule VariableLengthQuantity do
  import Bitwise
  @doc """
  Encode integers into a bitstring of VLQ encoded bytes
  """
  @spec encode(integers :: [integer]) :: binary
  def encode(integers) do
    integers
    |> Enum.flat_map(fn number -> get_parts(number) |> set_continuation_bits() end)
    |> :binary.list_to_bin()
  end

  defp get_parts(n) do # the first entry in the list will be the last byte
    if n == 0, do: [], else: [n &&& 0x7F | get_parts(n >>> 7)]
  end

  defp set_continuation_bits(parts) do
    if parts |> Enum.empty?() do
      [0]
    else # set the continuation bit in all entries except the first, then reverse the list
      parts
      |> Enum.with_index()
      |> Enum.map(fn {el, ind} -> if ind != 0, do: el ||| 0x80, else: el end)
      |> Enum.reverse
    end
  end

  @doc """
  Decode a bitstring of VLQ encoded bytes into a series of integers
  """
  @spec decode(bytes :: binary) :: {:ok, [integer]} | {:error, String.t()}
  def decode(bytes) do
    bytelist = :binary.bin_to_list(bytes)
    cond do
      bytelist |> Enum.empty?                              -> {:error, "empty binary"}
      bytelist |> Enum.reverse |> hd |> number_continues?  -> {:error, "incomplete sequence"}
      true                                                 -> {:ok, split(bytelist) |> Enum.map(&get_num/1)}
    end
  end

  defp number_continues?(b), do: (b &&& 0x80) == 0x80 # checks the continuation bit

  defp split(bytelist) do
    heads = bytelist |> Enum.take_while(&number_continues?/1) # so the last byte is not yet attached
    case bytelist |> Enum.drop(heads |> Enum.count) do
      [last | rest] -> [ heads ++ [last] | split(rest) ] # construct a list of lists here, attach the last byte to its heads
      _ -> []
    end
  end

  defp get_num(bytelist) do
    bytelist
    |> Enum.reverse
    |> Enum.with_index
    |> Enum.map(fn {el, ind} -> (el &&& 0x7F) <<< 7*ind end) # push the bits to the left by a multiple of 7
    |> Enum.sum
  end
end
