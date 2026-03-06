defmodule BafflingBirthdays do
  @moduledoc """
  Estimate the probability of shared birthdays in a group of people.
  """

  @spec shared_birthday?(birthdates :: [Date.t()]) :: boolean()
  def shared_birthday?(birthdates) do
    birthdates
    |> Enum.map(fn %{month: m, day: d} -> {m, d} end)
    |> Enum.group_by(& &1)
    |> Enum.any?(fn {_, g} -> length(g) > 1 end)
  end

  @spec random_birthdates(group_size :: integer()) :: [Date.t()]
  def random_birthdates(group_size) do
    (1..group_size) |> Enum.map(fn _ -> random_date_in_2025() end)
  end

  @spec estimated_probability_of_shared_birthday(group_size :: integer()) :: float()
  def estimated_probability_of_shared_birthday(group_size) do
    sims = 10000
    cnt = (1..sims) |> Enum.count(fn _ -> 
      shared_birthday?(random_birthdates(group_size)) end)
    cnt / sims * 100.0
  end

  defp random_date_in_2025() do
    month = :rand.uniform(12)
    Date.new!(2025, month, :rand.uniform(days_in_month(month)))
  end

  defp days_in_month(month) do
  %{2 => 28, 4 => 30, 6 => 30, 9 => 30, 11 => 30} |> Map.get(month, 31)
  end
end
