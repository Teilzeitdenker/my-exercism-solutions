defmodule PalindromeProducts do
  @doc """
  Generates all palindrome products from an optionally given min factor (or 1) to a given max factor.
  """
  @spec generate(non_neg_integer, non_neg_integer) :: map
  def generate(max_factor, min_factor \\ 1) do
    if max_factor < min_factor, do: raise ArgumentError
    min_factor..max_factor
    |> Stream.flat_map(fn i -> i..max_factor |> Enum.map(fn j -> [i, j] end)  end)
    |> Stream.filter(fn [a, b] -> palindrome?(a * b) end)
    |> Enum.group_by(fn [a, b] -> a * b end)
  end

  @spec palindrome?(non_neg_integer()) :: boolean()
  defp palindrome?(n) do
    n == rev(0, n)
  end

  @spec rev(non_neg_integer(), non_neg_integer()) :: non_neg_integer()
  defp rev(acc, 0), do: acc
  defp rev(acc, n), do: rev(acc*10 + rem(n, 10), div(n, 10))
end
