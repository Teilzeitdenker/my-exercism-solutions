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
      luhn_sum =
        trimmed
        |> String.codepoints
        |> Enum.map(&String.to_integer/1)
        |> Enum.reverse
        |> Enum.with_index
        |> Enum.map(fn {n, i}
          -> if rem(i, 2) == 1 do
              if n < 5 do
                2 * n
              else
                2 * n - 9
              end
            else
              n
            end
          end)
        |> Enum.sum
      rem(luhn_sum, 10) == 0
    end
  end
end
