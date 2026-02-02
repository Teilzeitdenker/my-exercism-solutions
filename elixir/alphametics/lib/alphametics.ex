defmodule Alphametics do
  @type puzzle :: binary
  @type solution :: %{required(?A..?Z) => 0..9}

  @doc """
  Takes an alphametics puzzle and returns a solution where every letter
  replaced by its number will make a valid equation. Returns `nil` when
  there is no valid solution to the given puzzle.

  ## Examples

    iex> Alphametics.solve("I + BB == ILL")
    %{?I => 1, ?B => 9, ?L => 0}

    iex> Alphametics.solve("A == B")
    nil
  """
  @spec solve(puzzle) :: solution | nil
  def solve(puzzle) do
    parsed = parse(puzzle)
    {full_map, non_zero_set} = place_values_and_non_zero_set(parsed)
    digits = 0..9 |> Enum.to_list
    num_chars = full_map |> Map.keys |> length
    all_perms = combinations(digits, num_chars) |> Enum.flat_map(&permutations/1)
    full_list = full_map |> Enum.to_list
    solution_perm = all_perms |> Enum.find(&is_solution(&1, full_list, non_zero_set))
    if solution_perm != nil do
      full_list |> Enum.map(&elem(&1, 0)) |> Enum.zip(solution_perm) |> Map.new
    else
      nil
    end
  end

  defp parse(puzzle) do
    puzzle |> String.split([" + ", " == "]) |> Enum.slide(-1, 0) |> then(fn [res | summands] -> {summands, res} end)
  end
  defp cons(x, ls), do: [x | ls]
  defp inserts(x, []), do: [[x]]
  defp inserts(x, ls = [y | ys]), do: [ [x | ls] | inserts(x, ys) |> Enum.map(&cons(y, &1)) ]
  defp permutations([]), do: [[]]
  defp permutations([x | xs]), do: permutations(xs) |> Enum.flat_map(&inserts(x, &1))
  defp combinations(_, 0), do: [[]]
  defp combinations([], _), do: []
  defp combinations([x| xs], k), do: (for p <- combinations(xs, k-1), do: [x | p]) ++ combinations(xs, k)
  defp process_folder(ch, {map, place_value}) do
    {map |> Map.update(ch, place_value, &Kernel.+(&1, place_value)), place_value * 10}
  end
  defp process(word, {place_value, old_map}) do
    new_map = word |> to_charlist |> Enum.reverse |> List.foldl({old_map, place_value}, &process_folder/2) |> elem(0)
    {place_value, new_map}
  end
  defp place_values_and_non_zero_set({summands, result}) do
    pre_map = summands |> List.foldl({1, Map.new}, &process/2) |> elem(1)
    full_map = result |> process({-1, pre_map}) |> elem(1)
    non_zero_set =
      (for word <- [result | summands], do: word |> to_charlist |> hd())
      |> List.foldl(MapSet.new, &MapSet.put(&2, &1))
    {full_map, non_zero_set}
  end
  defp is_solution(perm, full_list, non_zero_set) do
    if full_list |> Enum.map(&elem(&1, 1)) |> Enum.zip(perm) |> Enum.map(&Tuple.product/1) |> Enum.sum == 0 do
      zero_idx = perm |> Enum.find_index(fn x -> x == 0 end)
      if zero_idx == nil do
        true
      else
        not (non_zero_set |> MapSet.member?(full_list |> Enum.at(zero_idx) |> elem(0)))
      end
    else
      false
    end
  end
end
