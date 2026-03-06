defmodule BafflingBirthdays do
  @moduledoc """
  Estimate the probability of shared birthdays in a group of people.
  """
  @sims 10000
  def shared_birthday?(ls), do: Enum.map(ls, &extract_md/1) |> Enum.group_by(& &1) |> Enum.any?(&shared_group?/1)
  def random_birthdates(sz), do: Enum.take(Stream.repeatedly(&rand_date/0), sz)
  def estimated_probability_of_shared_birthday(sz), do: Enum.count(1..@sims,fn _ -> run_sim(sz) end) / @sims * 100.0

  defp rand_date(), do: Date.add(~D[2025-01-01], :rand.uniform(365) - 1)
  defp extract_md(%{month: m, day: d} = _date), do: {m, d}
  defp shared_group?({_key, group}), do: length(group) > 1
  defp run_sim(sz), do: shared_birthday?(random_birthdates(sz))
end
