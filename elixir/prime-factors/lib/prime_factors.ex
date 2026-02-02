defmodule PrimeFactors do
  @doc """
  Compute the prime factors for 'number'.

  The prime factors are prime numbers that when multiplied give the desired
  number.

  The prime factors of 'number' will be ordered lowest to highest.
  """
  @spec factors_for(pos_integer) :: [pos_integer]
  def factors_for(number) do
    get_factors(number)
  end

  defp get_factors(n, actual \\ 2, acc \\ [])
  defp get_factors(n, _actual, acc) when n <= 1, do: acc |> Enum.reverse()
  defp get_factors(n, actual, acc) do
    case rem(n, actual) do
      0 ->
        get_factors(div(n, actual), actual, [actual | acc])
      _ ->
        if actual == 2 do
          get_factors(n, 3, acc)
        else
          get_factors(n, actual + 2, acc)
        end
    end
  end
end
