defmodule Lasagna do
  def(expected_minutes_in_oven(), do: 40)
  def(remaining_minutes_in_oven(actual), do: expected_minutes_in_oven() - actual)
  def(preparation_time_in_minutes(layers), do: 2*layers)
  def(total_time_in_minutes(layers, actual), do: preparation_time_in_minutes(layers) + actual)
  def(alarm(), do: "Ding!")
end
