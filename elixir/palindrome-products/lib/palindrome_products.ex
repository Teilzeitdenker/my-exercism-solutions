defmodule PalindromeProducts do
  @doc """
  Generates all palindrome products from an optionally given min factor (or 1) to a given max factor.
  """
  @spec generate(non_neg_integer, non_neg_integer) :: map
  def generate(max_factor, min_factor \\ 1) do
    if max_factor < min_factor, do: raise ArgumentError
    (for i <- min_factor..max_factor, j <- i..max_factor, palindrome?(i*j), do: [i, j])
    |> Enum.group_by(fn [a, b] -> a * b end)
  end

  @spec palindrome?(non_neg_integer()) :: boolean()
  def palindrome?(n), do: n == rev(0, n)

  @spec rev(non_neg_integer(), non_neg_integer()) :: non_neg_integer()
  def rev(acc, 0), do: acc
  def rev(acc, n), do: rev(acc*10 + rem(n, 10), div(n, 10))
end
