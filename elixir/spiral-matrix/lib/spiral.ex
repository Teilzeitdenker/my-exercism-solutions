defmodule Spiral do
  @doc """
  Given the n, return a square matrix of numbers in clockwise spiral order.
  """
  @spec matrix(n :: integer) :: list(list(integer))
  def matrix(n) when n <= 0, do: []
  def matrix(1), do: [[1]]
  def matrix(n) do
    smaller = matrix(n-1) |> turn_around() |> add_all(2*n - 1)
    [ (1..n) |> Enum.to_list() | smaller |> Enum.with_index(fn row, ind -> row ++ [ind + n + 1] end)]
  end

  defp turn_around(ls), do: ls |> Enum.map(&Enum.reverse/1) |> Enum.reverse()
  defp add_all(ls, n),  do: ls |> Enum.map(fn row -> row |> Enum.map(fn el -> el + n end) end)
end
