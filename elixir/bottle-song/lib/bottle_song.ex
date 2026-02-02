defmodule BottleSong do
  @moduledoc """
  Handles lyrics of the popular children song: Ten Green Bottles
  """
  @numbers ["no", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"]

  @spec recite(pos_integer, pos_integer) :: String.t()
  def recite(start_bottle, take_down) do
    start_bottle..(start_bottle - take_down + 1) |> Enum.map_join("\n\n", &verse/1)
  end

  defp num_green_bottles(number) do
    num = Enum.at(@numbers, number)
    case number do
      1 -> {"#{num |> String.capitalize()} green bottle" , "#{num} green bottle"}
      _ -> {"#{num |> String.capitalize()} green bottles", "#{num} green bottles"}
    end
  end

  defp verse(number) do
    line1 = "#{elem(num_green_bottles(number), 0)} hanging on the wall,\n"
    line3 = "And if one green bottle should accidentally fall,\n"
    line4 = "There'll be #{elem(num_green_bottles(number - 1), 1)} hanging on the wall."
    line1 <> line1 <> line3 <> line4
  end
end
