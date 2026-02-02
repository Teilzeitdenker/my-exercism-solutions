defmodule PerfectNumbers do
  @doc """
  Determine the aliquot sum of the given `number`, by summing all the factors
  of `number`, aside from `number` itself.

  Based on this sum, classify the number as:

  :perfect if the aliquot sum is equal to `number`
  :abundant if the aliquot sum is greater than `number`
  :deficient if the aliquot sum is less than `number`
  """
  @spec classify(n :: integer) :: {:ok, atom} | {:error, String.t()}
  def classify(n) when n <= 0, do:
    {:error, "Classification is only possible for natural numbers."}
  def classify(n) do
    case divisor_sum(n) do
      x when x < n  -> {:ok, :deficient}
      x when x == n -> {:ok, :perfect}
      _             -> {:ok, :abundant}
    end
  end

  @spec divisor_sum(pos_integer()) :: pos_integer()
  defp divisor_sum(1), do: 0
  defp divisor_sum(n) do
    (1..div(n, 2)) |> Enum.filter(fn a -> rem(n, a) == 0 end) |> Enum.sum()
  end
end
