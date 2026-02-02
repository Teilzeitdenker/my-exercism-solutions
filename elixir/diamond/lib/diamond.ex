defmodule Diamond do
  @doc """
  Given a letter, it prints a diamond starting with 'A',
  with the supplied letter at the widest point.
  """
  @spec build_shape(char) :: String.t()
  def build_shape(last_letter) do
    # Build the upper part of the diamond, then use mirror symmetry
    upper_part_of_diamond =
      ?A..last_letter
      |> Enum.map(&get_row(&1, last_letter - ?A))
      |> Enum.map(&to_string/1)
      |> Enum.map(fn s -> s <> "\n" end)
    lower_part_of_diamond = upper_part_of_diamond |> Enum.reverse() |> tl
    upper_part_of_diamond ++ lower_part_of_diamond |> Enum.join("")
  end

  defp get_row(letter, n) do
    # Also use symmetry to build up a row. Reflect the tail of the right part to the left.
    # Build the right part of each row by sliding each letter by its own place number in the alphabet through the whitespaces (ASCII value: 32)
    right_part = [letter | List.duplicate(32, n)] |> Enum.slide(0, letter - ?A)
    left_part  = tl(right_part) |> Enum.reverse()
    left_part ++ right_part
  end
end
