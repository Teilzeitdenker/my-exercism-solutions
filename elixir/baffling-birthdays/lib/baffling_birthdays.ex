defmodule BafflingBirthdays do
  @moduledoc """
  Estimate the probability of shared birthdays in a group of people.
  """
  @simulations 10000

  @spec shared_birthday?(birthdates :: [Date.t()]) :: boolean()
  def shared_birthday?(birthdates) do
    birthdates
    |> Enum.map(fn %{month: m, day: d} -> {m, d} end)
    |> Enum.group_by(& &1)
    |> Enum.any?(fn {_, g} -> length(g) > 1 end)
  end

  @spec random_birthdates(group_size :: integer()) :: [Date.t()]
  def random_birthdates(group_size) do
    (1..group_size) |> Enum.map(fn _ -> 
    Date.add(~D[2025-01-01], :rand.uniform(365) - 1) end)
  end

  @spec estimated_probability_of_shared_birthday(group_size :: integer()) :: float()
  def estimated_probability_of_shared_birthday(group_size) do
    cnt = (1..@simulations) |> Enum.count(fn _ -> 
      shared_birthday?(random_birthdates(group_size)) end)
    cnt / @simulations * 100.0
  end
end
