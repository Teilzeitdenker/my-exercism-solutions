defmodule Connect do
  @offsets [{-1, 0}, {-1, 1}, {0, -1}, {0, 1}, {1, -1}, {1, 0}]
  @doc """
  Calculates the winner (if any) of a board
  using "O" as the white player
  and "X" as the black player
  """
  @spec result_for([String.t()]) :: :none | :black | :white
  def result_for(board) do
    cond do
      0..(length(board) - 1) |> Enum.any?(fn r ->
        path?(board, r, 0, ?X, [], &reached_black_goal/3) end) -> :black
      0..(width(board) - 1)  |> Enum.any?(fn c ->
        path?(board, 0, c, ?O, [], &reached_white_goal/3) end) -> :white
      true -> :none
    end
  end

  defp width(board), do: String.length(Enum.at(board, 0))
  defp on_board?(board, r, c) do
    r >= 0 && r < length(board) && c >= 0 && c < width(board)
  end
  defp path?(board, r, c, stone, visited, reached_player_goal) do
    cond do
      visited |> Enum.member?({r, c}) -> false
      not(on_board?(board, r, c)) or
        board |> Enum.at(r) |> to_charlist |> Enum.at(c) != stone -> false
      reached_player_goal.(board, r, c) -> true
      true -> @offsets |> Enum.any?(fn {dr, dc} ->
        path?(board, r + dr, c + dc, stone, [{r, c}|visited], reached_player_goal)
      end)
    end
  end
  defp reached_black_goal(board, _r, c), do: c == width(board) - 1
  defp reached_white_goal(board, r, _c), do: r == length(board) - 1
end
