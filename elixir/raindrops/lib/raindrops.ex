defmodule Raindrops do
  @spec divisible(pos_integer(), pos_integer()) :: String.t()
  def divisible(number, divisor) do
    if rem(number, divisor) == 0 do
      case divisor do
        3 -> "Pling"
        5 -> "Plang"
        7 -> "Plong"
        _ -> ""
      end
    else
      ""
    end

  end
  @doc """
  Returns a string based on raindrop factors.

  - If the number contains 3 as a prime factor, output 'Pling'.
  - If the number contains 5 as a prime factor, output 'Plang'.
  - If the number contains 7 as a prime factor, output 'Plong'.
  - If the number does not contain 3, 5, or 7 as a prime factor,
    just pass the number's digits straight through.
  """
  @spec convert(pos_integer()) :: String.t()
  def convert(number) do
    divisors = [3, 5, 7]
    result = divisors |> Enum.reduce("", fn divisor, acc -> acc <> divisible(number, divisor) end)
    if result |> String.length > 0 do
      result
    else
      "#{number}"
    end
  end
end
