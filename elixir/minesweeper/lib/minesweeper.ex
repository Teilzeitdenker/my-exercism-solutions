defmodule Minesweeper do
  @doc """
  Annotate empty spots next to mines with the number of mines next to them.
  """

  @spec annotate([String.t()]) :: [String.t()]
  def annotate([]), do: []
  def annotate([""]), do: [""]
  def annotate(board) do
    cols = board |> Enum.at(0) |> String.length()
    board
    |> Enum.map(&get_flowers(&1, cols))
    |> Enum.reduce(fn row_flower, acc ->
      (acc |> Enum.drop(-2)) ++ [
        add_arrs(acc |> Enum.at(-2), row_flower |> Enum.at(-3)),
        add_arrs(acc |> Enum.at(-1), row_flower |> Enum.at(-2)),
        row_flower |> Enum.at(-1)
      ]
    end)
    |> then(&clean_up/1)
  end

  defp get_flowers(row, cols) do
    default_list = List.duplicate(List.duplicate(0, cols + 2), 3)
    row
    |> String.codepoints
    |> Enum.with_index
    |> Enum.filter(&(elem(&1, 0) == "*"))
    |> Enum.map(fn {_, col} -> flower(col, cols) end)
    |> Enum.reduce(default_list, &add_flowers/2)
  end

  defp flower(col, cols) do
    edge = [1, 1, 1] ++ List.duplicate(0, cols - 1)
    middle = [1, -1, 1] ++ List.duplicate(0, cols - 1)
    if col == 0 do
      [edge, middle, edge]
    else
      edge = Enum.slide(edge, 0..2, col + 2)
      middle = Enum.slide(middle, 0..2, col + 2)
      [edge, middle, edge]
    end
  end

  defp add_flowers(flower1, flower2) do
    Enum.zip(flower1, flower2) |> Enum.map(fn {arr1, arr2} -> add_arrs(arr1, arr2) end)
  end

  defp add_arrs(arr1, arr2) do
    Enum.zip(arr1, arr2) |> Enum.map(fn {el1, el2} -> add_nums(el1, el2) end)
  end

  defp add_nums(el1, el2) when el1 < 0 or el2 < 0, do: -1
  defp add_nums(el1, el2), do: el1 + el2

  defp clean_up(result_rows) do
    result_rows |> Enum.drop(1) |> Enum.drop(-1) |> Enum.map(&get_result_string/1)
  end

  defp get_result_string(row) do
    row |> Enum.drop(1)|> Enum.drop(-1)
    |> Enum.map(fn num ->
      case num do
        -1 -> "*"
        0  -> " "
        n  -> Integer.to_string(n)
      end
    end)
    |> Enum.join
  end
end
