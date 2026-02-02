defmodule Triplet do
  @doc """
  Calculates sum of a given triplet of integers.
  """
  @spec sum([non_neg_integer]) :: non_neg_integer
  def sum(triplet) do
    triplet |> Enum.sum()
  end

  @doc """
  Calculates product of a given triplet of integers.
  """
  @spec product([non_neg_integer]) :: non_neg_integer
  def product(triplet) do
    triplet |> Enum.product()
  end

  @doc """
  Determines if a given triplet is pythagorean. That is, do the squares of a and b add up to the square of c?
  """
  @spec pythagorean?([non_neg_integer]) :: boolean
  def pythagorean?([a, b, c]) do
    a*a + b*b == c*c
  end

  @doc """
  Generates a list of pythagorean triplets whose values add up to a given sum.
  """
  @spec generate(non_neg_integer) :: [list(non_neg_integer)]
  def generate(sum) do
    1..div(sum, 3) |> Enum.reduce([], fn a, acc ->
      bs = (a + 1)..div(sum - a, 2) |> Enum.filter(fn b ->
        pythagorean?([a, b, sum - a - b])
      end)
      if bs |> Enum.count() > 0, do:  acc ++ [[a, List.first(bs), sum - a - List.first(bs)]], else: acc
    end)
  end
end
