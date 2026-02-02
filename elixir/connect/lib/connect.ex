defmodule Connect do
  @doc """
  Calculates the winner (if any) of a b
  using "O" as the white player
  and "X" as the black player
  """
  @spec result_for([String.t()]) :: :none | :black | :white
  def result_for(b) do
    cond do
      0..(length(b) - 1) |> Enum.any?(fn r ->
        path?(b, r, 0, ?X, [], &reached_black_goal/3) end) -> :black
      0..(width(b) - 1)  |> Enum.any?(fn c ->
        path?(b, 0, c, ?O, [], &reached_white_goal/3) end) -> :white
      true -> :none
    end
  end

  defp width(b), do: String.length(Enum.at(b, 0))
  defp on_board?(b, r, c), do: r >= 0 && r < length(b) && c >= 0 && c < width(b)
  defp entry(b, r, c), do: b |> Enum.at(r) |> to_charlist |> Enum.at(c)
  defp path?(b, r, c, stone, visited, reached_player_goal) do
    cond do
      visited |> Enum.member?({r, c})                    -> false
      not(on_board?(b, r, c)) or entry(b, r, c) != stone -> false
      reached_player_goal.(b, r, c)                      -> true
      true                                               ->
        [{-1, 0},{-1, 1},{0, -1},{0, 1},{1, -1},{1, 0}] |> Enum.any?(fn {dr, dc} ->
          path?(b, r + dr, c + dc, stone, [{r, c}|visited], reached_player_goal)
        end)
    end
  end
  defp reached_black_goal(b, _r, c), do: c == width(b) - 1
  defp reached_white_goal(b, r, _c), do: r == length(b) - 1
end
