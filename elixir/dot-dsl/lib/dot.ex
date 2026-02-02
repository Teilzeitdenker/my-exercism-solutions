defmodule Dot do
  defmacro graph(do: ast) do
    with {_, graph} = Macro.prewalk(ast, Graph.new(), &walk_graph/2), do: Macro.escape(graph)
  end

  defp walk_graph({:graph, _, [attrs]}, graph), do: {{}, Graph.put_attrs(graph, attrs)}
  defp walk_graph({node, _, nil}, graph), do: {{}, Graph.add_node(graph, node)}

  defp walk_graph({node, _, [attrs]}, graph) when is_list(attrs),
    do: {{}, Graph.add_node(graph, node, attrs)}

  defp walk_graph({:--, _, [{from, _, _}, {to, _, nil}]}, graph),
    do: {{}, Graph.add_edge(graph, from, to)}

  defp walk_graph({:--, _, [{from, _, _}, {to, _, [attrs]}]}, graph) when is_list(attrs),
    do: {{}, Graph.add_edge(graph, from, to, attrs)}

  defp walk_graph({_, _, _} = ast, graph), do: {ast, graph}

  defp walk_graph(_, _), do: raise(ArgumentError)
end
