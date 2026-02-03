defmodule RelativeDistance do
  @doc """
  Find the degree of separation of two members given a given family tree.
  """
  @spec degree_of_separation(
          family_tree :: %{String.t() => [String.t()]},
          person_a :: String.t(),
          person_b :: String.t()
        ) :: nil | pos_integer()
  def degree_of_separation(family_tree, person_a, person_b) do
    family_tree |> get_graph() # start queue at person_a with count = 0
    |> get_shortest_path(person_b, [{person_a, 0}], MapSet.new())
  end

  defp get_graph(tr) do # mathematically, this is not a tree (which must be acyclic)
    for {pr, chs} <- tr, ch <- chs, reduce: %{} do
      graph ->
        sbs = chs |> MapSet.new() |> MapSet.delete(ch) # siblings 
        graph 
        |> Map.update(pr, MapSet.new([ch]), &MapSet.put(&1, ch)) # top - down
        |> Map.update(ch, MapSet.new([pr]), &MapSet.put(&1, pr)) # down - top
        |> Map.update(ch, sbs, &MapSet.union(&1, sbs)) # horizontal connections
    end
  end
  
  defp get_shortest_path(_, _, [], _), do: nil # queue empty, no connection
  defp get_shortest_path(_, hit, [{hit, cnt} | _], _), do: cnt # hit the right person
  defp get_shortest_path(gr, hit, [{curr, cnt} | q], prev) do
    prev = prev |> MapSet.put(curr)
    nxt = gr[curr] 
    |> MapSet.reject(fn p -> MapSet.member?(prev, p) end)
    |> Enum.map(fn p -> {p, cnt + 1} end) 
    get_shortest_path(gr, hit, q ++ nxt, prev) 
  end
end
