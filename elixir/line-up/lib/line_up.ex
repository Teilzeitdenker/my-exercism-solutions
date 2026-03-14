defmodule LineUp do
  def format(name, number), do:
    "#{name}, you are the #{number}#{suffix(number)} customer we serve today. Thank you!"
  defp suffix(n) when rem(n, 10) == 1 and rem(n, 100) != 11, do: "st"
  defp suffix(n) when rem(n, 10) == 2 and rem(n, 100) != 12, do: "nd"
  defp suffix(n) when rem(n, 10) == 3 and rem(n, 100) != 13, do: "rd"
  defp suffix(_), do: "th"
end
