defmodule PascalsTriangle do
  @doc """
  Calculates the rows of a pascal triangle
  with the given height
  """
  @spec rows(integer) :: [[integer]]
  def rows(num) do
    case num do
      0 -> []
      1 -> [[1]]
      n ->
        before = rows(n - 1)
        inner_part_of_new_line = before |> List.last() |> Enum.chunk_every(2, 1, :discard) |> Enum.map(&Enum.sum/1)
        before ++ [[1 | inner_part_of_new_line] ++ [1]]
    end
  end
end
