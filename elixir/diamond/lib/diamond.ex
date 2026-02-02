defmodule Diamond do
  @doc """
  Given a letter, it prints a diamond starting with 'A',
  with the supplied letter at the widest point.
  """
  @spec build_shape(char) :: String.t()
  def build_shape(?A), do: "A\n"
  def build_shape(letter) do
    rows = (?A..(letter - 1) |> Enum.to_list()) ++ ((letter..?A//-1) |> Enum.to_list())
    rows
    |> Enum.map(&get_row(&1, letter - ?A))
    |> Enum.map(&to_string/1)
    |> Enum.map(fn s -> s <> "\n" end)
    |> Enum.join("")
  end

  defp get_row(c, n) do
    right_half = [c | List.duplicate(32, n)] |> Enum.slide(0, c - ?A)
    ( right_half |> Enum.slice(1..-1//1) |> Enum.reverse ) ++ right_half
  end

end
