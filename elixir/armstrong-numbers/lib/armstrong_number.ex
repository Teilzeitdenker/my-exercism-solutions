
defmodule ArmstrongNumber do
  @moduledoc """
  Provides a way to validate whether or not a number is an Armstrong number
  """

  @spec valid?(integer) :: boolean
  def valid?(number) do
    digits = to_charlist(number)
    exponent = length(digits)
    digits
    |> Enum.map(fn c -> c - ?0 end)
    |> Enum.map(fn n -> n ** exponent end)
    |> Enum.sum() == number
  end
end
