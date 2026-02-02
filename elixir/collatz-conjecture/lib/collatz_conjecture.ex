defmodule CollatzConjecture do
  @doc """
  calc/1 takes an integer and returns the number of steps required to get the
  number to 1 when following the rules:
    - if number is odd, multiply with 3 and add 1
    - if number is even, divide by 2
  """
  def calc(n) when is_integer(n) and n > 0 do
    cond do
      n == 1 -> 0
      rem(n, 2) == 0 -> 1 + calc(div(n, 2))
      true -> 1 + calc(3*n + 1)
    end
  end
end
