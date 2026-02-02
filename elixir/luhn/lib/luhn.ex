defmodule Luhn do
  @doc """
  Checks if the given number is valid via the luhn formula
  """
  @spec valid?(String.t()) :: boolean
  def valid?(number) do
    trimmed = number |> String.replace(~r/\s/, "")
    if trimmed =~ ~r/\D/ or trimmed |> String.length < 2 do
      false
    else
      trimmed
      |> String.codepoints
      |> Enum.map(&String.to_integer/1)
      |> Enum.reverse
      |> Enum.with_index
      |> Enum.map(&luhn_double/1)
      |> Enum.sum
      |> rem(10) == 0
    end
  end

  defp luhn_double({n, i}) do
    if rem(i, 2) == 1 do
      if n < 5, do: 2 * n, else: 2 * n - 9
    else
      n
    end
  end
end
