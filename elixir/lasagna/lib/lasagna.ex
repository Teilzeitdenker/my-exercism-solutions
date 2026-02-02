defmodule Lasagna do
  # Please define the 'expected_minutes_in_oven/0' function
  def(expected_minutes_in_oven(), do: 40)
  # Please define the 'remaining_minutes_in_oven/1' function
  def(remaining_minutes_in_oven(actual), do: 40 - actual)
  # Please define the 'preparation_time_in_minutes/1' function
  def(preparation_time_in_minutes(layers), do: 2*layers)
  # Please define the 'total_time_in_minutes/2' function
  def(total_time_in_minutes(layers, actual), do: preparation_time_in_minutes(layers) + actual)
  # Please define the 'alarm/0' function
  def(alarm(), do: "Ding!")
end
