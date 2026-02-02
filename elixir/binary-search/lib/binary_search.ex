defmodule BinarySearch do
  @doc """
    Searches for a key in the tuple using the binary search algorithm.
    It returns :not_found if the key is not in the tuple.
    Otherwise returns {:ok, index}.

    ## Examples

      iex> BinarySearch.search({}, 2)
      :not_found

      iex> BinarySearch.search({1, 3, 5}, 2)
      :not_found

      iex> BinarySearch.search({1, 3, 5}, 5)
      {:ok, 2}

  """

  @spec search(tuple, integer) :: {:ok, integer} | :not_found
  def search(numbers, key) do
    find(0, tuple_size(numbers) - 1, numbers, key)
  end

  defp find(lo, hi, numbers, key) do
    if hi < lo do
      :not_found
    else
      mid = div(lo + hi, 2)
      cand = numbers |> elem(mid)
      cond do
        cand == key -> {:ok, mid}
        cand < key -> find(mid + 1, hi, numbers, key)
        true -> find(lo, mid - 1, numbers, key)
      end
    end
  end
end
