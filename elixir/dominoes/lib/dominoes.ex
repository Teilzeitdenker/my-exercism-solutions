defmodule Dominoes do
  @type domino :: {1..6, 1..6}

  @doc """
  chain?/1 takes a list of domino stones and returns boolean indicating if it's
  possible to make a full chain
  """
  @spec chain?(dominoes :: [domino]) :: boolean
  def chain?(dominoes) do
    case dominoes do
      [] -> true
      [{a, b}] -> a == b
      [x | xs] -> get_chains(x, xs) |> Enum.any?(&chain?/1)
    end
  end

  defp get_chains(x, xs) do
    (for n <- 1 .. length(xs), do: Enum.slide(xs, n - 1, 0))
    |> Enum.filter(&does_chain?(&1, x))
    |> Enum.map(&do_chain(&1, x))
  end

  defp does_chain?([{a, b} | _], {_, en}) do
    if en == a or en == b, do: true, else: false
  end

  # When using this, one already knows that the dominoes match, so else case is just switching
  defp do_chain([{a, b} | ds], {st, en}) do
    if en == a, do: [{st, b} | ds], else: [{st, a} | ds]
  end
end
