defmodule RomanNumerals do
  @roman_letters [{"I", "V"}, {"X", "L"}, {"C", "D"}, {"M", "?"}]
  @doc """
  Convert the number to a roman number.
  """
  @spec numeral(pos_integer) :: String.t()
  def numeral(number) do
    if number < 0 || number >= 4000, do: raise "no conversion to roman for this number"
    Integer.digits(number)
    |> Enum.reverse()
    |> Enum.with_index()
    |> Enum.reduce("", fn el, acc -> get_number(el) <> acc end)
  end

  defp get_number({digit, pos}) do
    {a, b} = @roman_letters |> Enum.at(pos)
    case digit do
      n when n in [0, 1, 2, 3] -> String.duplicate(a, n)
      4 -> a <> b
      r when r in [5, 6, 7, 8] -> b <> String.duplicate(a, r - 5)
      9 -> a <> (@roman_letters |> Enum.at(pos + 1) |> elem(0))
      _ -> raise "not a digit"
    end
  end
end
