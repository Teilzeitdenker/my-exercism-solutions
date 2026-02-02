defmodule DNA do
  def encode_nucleotide(code_point) do
    cond do
      code_point == ?A -> 0b0001
      code_point == ?C -> 0b0010
      code_point == ?G -> 0b0100
      code_point == ?T -> 0b1000
      code_point == ?\s -> 0b0000
    end
  end

  def decode_nucleotide(encoded_code) do
    cond do
      encoded_code == 0b0001 -> ?A
      encoded_code == 0b0010 -> ?C
      encoded_code == 0b0100 -> ?G
      encoded_code == 0b1000 -> ?T
      encoded_code == 0b0000 -> ?\s
    end
  end

  def encode([]) do
    <<>>
  end
  def encode([head | tail]) do
    <<encode_nucleotide(head)::4, encode(tail)::bitstring>>
  end

  # use an accumulator list
  def decode(dna), do: do_decode(dna, [])

  defp do_decode(<<>>, acc), do: acc
  defp do_decode(<<head::4, tail::bitstring>>, acc), do: do_decode(tail, acc ++ [decode_nucleotide(head)]  )
end
