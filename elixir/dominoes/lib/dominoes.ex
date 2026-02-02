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
      [x | xs] -> getNxtChains(x, xs) |> Enum.any?(&chain?/1)
    end
  end

  defp getNxtChains(x, xs) do
    (for n <- 1 .. length(xs), do: Enum.slide(xs, n - 1, 0))
    |> Enum.filter(&doesChain?(&1, x))
    |> Enum.map(&doChain(&1, x))
  end

  defp doesChain?([{a, b} | _], {_, en}) do
    if en == a or en == b, do: true, else: false
  end

  # When using this, one already knows that the dominoes match, so else case is just switching
  defp doChain([{a, b} | ds], {st, en}) do
    if en == a, do: [{st, b} | ds], else: [{st, a} | ds]
  end
end
