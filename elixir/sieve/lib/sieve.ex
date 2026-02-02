defmodule Sieve do
  @doc """
  Generates a list of primes up to a given limit.
  """
  @spec primes_to(non_neg_integer) :: [non_neg_integer]
  def primes_to(limit) when limit < 2, do: []
  def primes_to(2), do: [2]
  def primes_to(limit) do
    sieve([2], 3..limit//2 |> Enum.to_list)
  end

  @spec sieve([non_neg_integer()], [non_neg_integer()]) :: [non_neg_integer()]
  defp sieve(primes, []), do: primes |> Enum.reverse()
  defp sieve(primes, [p | unsieved]) do
     sieve( [p | primes],
            unsieved
            |> Enum.reject(fn el -> el >= p * p && rem(el, p) == 0 end)
          )
  end
end
