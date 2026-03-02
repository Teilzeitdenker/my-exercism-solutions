defmodule Transmission do
  import Bitwise
  @doc """
  Return the transmission sequence for a message.
  """
  @spec get_transmit_sequence(bitstring()) :: binary()
  def get_transmit_sequence(<<>>), do: <<>>
  def get_transmit_sequence(<<bits::7, rest::bitstring>>) do
    <<encode_byte(bits), get_transmit_sequence(rest)::binary>>
  end
  def get_transmit_sequence(remaining) do 
    sz = bit_size(remaining)
    <<bits::size(sz)>> = remaining # bits is now an integer
    <<encode_byte(bits <<< (7 - sz))>> # shift it to the left
  end

  defp encode_byte(bits) do 
    parity = count_ones(bits) &&& 1
    (bits <<< 1) ||| parity
  end

  defp count_ones(0), do: 0
  defp count_ones(n) do
    (n &&& 1) + count_ones(n >>> 1)
  end

  @doc """
  Return the message decoded from the received transmission.
  """
  @spec decode_message(binary()) :: {:ok, binary()} | {:error, String.t()}
  def decode_message(received_data) do
    if valid_parity?(received_data) do
      {:ok, do_decode_message(received_data, <<>>)}
    else
      {:error, "wrong parity"}
    end
  end

  defp valid_parity?(<<>>), do: true
  defp valid_parity?(<<byte, rest::binary>>) do
    (count_ones(byte) &&& 1) == 0 and valid_parity?(rest)
  end

  defp do_decode_message(<<>>, acc) do
    # extract only full bytes of the accumulator
    bytes = div(bit_size(acc), 8)
    <<result::binary-size(bytes), _::bitstring>> = acc
    result
  end
  defp do_decode_message(<<byte, rest::binary>>, acc) do
    # remove parity bit by shifting and add this to bitstring
    do_decode_message(rest, <<acc::bitstring, (byte >>> 1)::7>>)
  end
end
