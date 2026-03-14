defmodule LineUp do
  @doc """
  Formats a full ticket sentence for the given name and number, including
  the person's name, the ordinal form of the number, and fixed descriptive text.
  """
  @spec format(name :: String.t(), number :: pos_integer()) :: String.t()
  def format(name, number) do
    "#{name}, you are the #{number}#{suffix(number)} customer we serve today. Thank you!"
  end

  defp suffix(n) do
    case n do
      n when rem(n, 100) >= 11 and rem(n, 100) <= 13 -> "th"
      n when rem(n, 10) == 1 -> "st"
      n when rem(n, 10) == 2 -> "nd"
      n when rem(n, 10) == 3 -> "rd"
      _ -> "th"
    end
  end
end
