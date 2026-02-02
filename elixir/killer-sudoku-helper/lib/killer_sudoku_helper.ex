defmodule KillerSudokuHelper do
  @doc """
  Return the possible combinations of `size` distinct numbers from 1-9 excluding `exclude` that sum up to `sum`.
  """
  @spec combinations(cage :: %{exclude: [integer], size: integer, sum: integer}) :: [[integer]]
  def combinations(cage) do
    take_only = (1 .. 9) |> Enum.filter(fn e -> not Enum.member?(cage[:exclude], e) end)
    first_candidates = take_only |> Enum.map(fn e -> [e] end)
    do_combinations(%{cage | exclude: take_only}, first_candidates)
  end

  defp do_combinations(%{size: size, sum: sum}, candidates) when size == 1, do: candidates |> Enum.filter(&(Enum.sum(&1) == sum)) |> Enum.map(&Enum.reverse/1)
  defp do_combinations(%{exclude: take_only, size: size, sum: sum} = cage, candidates) do
    new_candidates =
      candidates
      |> Enum.flat_map(fn [fst | _rest] = candidate -> take_only |> Enum.filter(&(&1 > fst)) |> Enum.map(&[&1|candidate]) end)
      |> Enum.filter(&(Enum.sum(&1) <= sum))
    do_combinations(%{cage | size: size - 1}, new_candidates)
  end
end
