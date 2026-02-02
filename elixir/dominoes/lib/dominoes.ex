defmodule Dominoes do
  @type domino :: {1..6, 1..6}

  @doc """
  chain?/1 takes a list of domino stones and returns boolean indicating if it's
  possible to make a full chain
  """
  @spec chain?(dominoes :: [domino]) :: boolean
  def chain?([]), do: true
  def chain?([{a, b}]), do: a == b
  def chain?([x | xs]), do: get_next_chains(x, xs) |> Enum.any?(&chain?/1)

  defp get_next_chains(x, xs), do: (for n <- 1 .. length(xs), do: Enum.slide(xs, n - 1, 0)) |> Enum.filter(&does_chain?(&1, x)) |> Enum.map(&do_chain(&1, x))
  defp does_chain?([{a, b} | _], {_, en}), do: if en == a or en == b, do: true, else: false
  defp do_chain([{a, b} | ds], {st, en}), do: if en == a, do: [{st, b} | ds], else: [{st, a} | ds]
end
