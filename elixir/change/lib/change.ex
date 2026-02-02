defmodule Change do
  @doc """
    Determine the least number of coins to be given to the user such
    that the sum of the coins' value would equal the correct amount of change.
    It returns {:error, "cannot change"} if it is not possible to compute the
    right amount of coins. Otherwise returns the tuple {:ok, list_of_coins}

    ## Examples

      iex> Change.generate([5, 10, 15], 3)
      {:error, "cannot change"}

      iex> Change.generate([1, 5, 10], 18)
      {:ok, [1, 1, 1, 5, 10]}

  """

  @spec generate(list, integer) :: {:ok, list} | {:error, String.t()}
  def generate(coins, target) do
    case target do
      _ when target < 0 -> {:error, "cannot change"}
      0                 -> {:ok, []}
      _                 ->
        res = Enum.to_list(1..target)
          |> Enum.reduce(%{0 => []}, &Map.put(&2, &1, get_next_coin_list(&1, &2, coins)))
          |> Map.fetch!(target)
        if res != nil, do: {:ok, res}, else: {:error, "cannot change"}
    end
  end

  defp get_next_coin_list(amount, map, coins) do
    coins
    |> Enum.filter(& &1 <= amount)
    |> Enum.map(&append_coin_to_prev_lists(amount, map, &1))
    |> Enum.filter(& &1 != nil)
    |> take_shortest_list
  end

  defp take_shortest_list(list_of_lists) do
    case list_of_lists do
      [] -> nil
      _  -> list_of_lists |> Enum.min_by(&length/1)
    end
  end

  defp append_coin_to_prev_lists(amount, map, coin) do
    ls = map |> Map.fetch!(amount - coin)
    if ls != nil, do: [coin | ls], else: nil
  end
end
