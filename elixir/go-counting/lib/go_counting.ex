defmodule GoCounting do
  @type position :: {integer, integer}
  @type owner :: %{owner: atom, territory: [position]}
  @type territories :: %{white: [position], black: [position], none: [position]}

  @doc """
  Return the owner and territory around a position
  """
  @spec territory(board :: String.t(), position :: position) ::
          {:ok, owner} | {:error, String.t()}
  def territory(board, {x, y}) do
    b = board |> String.split("\n", trim: true) |> Enum.map(&String.to_charlist/1)
    cond do
      not_on_board?(b, x, y) -> {:error, "Invalid coordinate"}
      entry(b, x, y) != ?_   -> {:ok, %{owner: :none, territory: []}}
      true                   -> {entries, coords} = territory(b, x, y, [])
        owner = case Enum.uniq(entries), do:
          ([?B] -> :black; [?W] -> :white; _ -> :none)
        {:ok, %{owner: owner, territory: Enum.sort(Enum.uniq(coords))}}
    end
  end

  @doc """
  Return all white, black and neutral territories
  """
  @spec territories(board :: String.t()) :: territories
  def territories(board) do
    b = board |> String.split("\n", trim: true)
    for y <- 0..(length(b)-1), x <- 0..(String.length(hd(b))-1) do
       territory(board, {x, y}) |> elem(1)
    end
    |> Enum.group_by(&(&1.owner), &(&1.territory))
    |> Map.new(fn {k, v} -> {k, Enum.sort(Enum.uniq(Enum.concat(v)))} end)
    |> then(&Map.merge(%{black: [], none: [], white: []}, &1))
  end

  defp not_on_board?(b, x, y), do: y<0 || y>=length(b) || x<0 || x>=length(hd(b))
  defp entry(b, x, y), do: b |> Enum.at(y) |> Enum.at(x)
  defp territory(b, x, y, done) do
    cond do
      not_on_board?(b, x, y) -> {[], []}
      {x, y} in done         -> {[], []}
      true -> owner = entry(b, x, y)
              if owner != ?_, do: {[owner], []}, else:
                Enum.reduce([{0, 1}, {0, -1}, {1, 0}, {-1, 0}], {[], [{x, y}]},
                  fn {x1, y1}, {entries, coords} ->
                    {e, c} = territory(b, x + x1, y + y1, [{x, y}|done])
                    {entries ++ e, coords ++ c}
                  end)
    end
  end
end
