defmodule GameOfLife do
  @offsets [{-1, 0}, {1, 0}, {0, -1}, {0, 1}, {-1, -1}, {-1, 1}, {1, -1}, {1, 1}]
  @doc """
  Apply the rules of Conway's Game of Life to a grid of cells
  """

  @spec tick(mat :: list(list(0 | 1))) :: list(list(0 | 1))
  def tick(mat) do
    mat |> Enum.with_index |> Enum.map(fn {row, i} ->
      row |> Enum.with_index |> Enum.map(fn {alive?, j} ->
        apply_rules(i, j, alive?, mat)
      end)
    end)
  end

  defp get_value(i, j, mat) do # simply returns 0 for indices out of range
    {h, w} = {length(mat), length(mat |> Enum.at(0))}
    if i < 0 or j < 0 or i >= h or j >= w, do: 0, else: mat |> Enum.at(i) |> Enum.at(j)
  end

  defp num_alive_ngbs(i, j, mat) do
    @offsets |> Enum.reduce(0, fn {di, dj}, sum -> sum + get_value(i + di, j + dj, mat) end)
  end

  defp apply_rules(i, j, alive?, mat) do
    case {alive?, num_alive_ngbs(i, j, mat)} do
      {_, 3} -> 1 # stasis for alive cells, reproduction for dead cells
      {1, 2} -> 1 # stasis for alive cells
      _      -> 0 # stasis for dead  cells, under- and overpopulation for alive cells
    end
  end
end
