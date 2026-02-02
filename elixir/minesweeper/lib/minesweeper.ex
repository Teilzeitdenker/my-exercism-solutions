defmodule Minesweeper do
  @doc """
  Annotate empty spots next to mines with the number of mines next to them.
  """
  @spec annotate([String.t()]) :: [String.t()]

  def annotate(board) do
    if board |> Enum.empty?() || board == [""] do
      board
    else
      cols = board |> Enum.at(0) |> String.length()
      default_list = List.duplicate(List.duplicate(0, cols + 2), 3)
      board
        |> Enum.map(&get_flowers(&1, cols))
        |> Enum.reduce({[], default_list}, &get_next_acc/2)
        |> then(&add_last_lines_and_clean_up/1)
    end
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

  defp add_nums(el1, el2) do
    if el1 < 0 || el2 < 0 do
      -1
    else
      el1 + el2
    end
  end

  defp get_next_acc(row, {result, actual}) do
    {new_row, next_actual} = add_rows(actual, row)
    {
      result ++ [get_result_string(new_row)],
      next_actual
    }
  end

  defp add_rows(row_flowers1, row_flowers2) do
    {
      row_flowers1 |> Enum.at(0),
      [
        add_arrs(row_flowers1 |> Enum.at(1), row_flowers2 |> Enum.at(0)),
        add_arrs(row_flowers1 |> Enum.at(2), row_flowers2 |> Enum.at(1)),
        row_flowers2 |> Enum.at(2)
      ]
    }
  end

  defp get_result_string(row) do
    row
    |> Enum.drop(1)
    |> Enum.drop(-1)
    |> Enum.map(fn num ->
      case num do
        -1 -> "*"
        0  -> " "
        n  -> to_string(n)
      end
    end)
    |> Enum.join
  end

  defp add_last_lines_and_clean_up({not_yet, last_lines}) do
    cleaned_last_lines = last_lines |> Enum.drop(-1) |> Enum.map(&get_result_string/1)
    not_yet ++ cleaned_last_lines |> Enum.drop(2)
  end
end
