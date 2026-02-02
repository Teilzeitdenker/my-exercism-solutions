defmodule SquareRoot do
  @doc """
  Calculate the integer square root of a positive integer
  """
  @spec calculate(radicand :: pos_integer) :: pos_integer
  def calculate(radicand) do
    isqrt(radicand, seed(radicand))
  end

  # see https://en.wikipedia.org/wiki/Integer_square_root in the subsection "Using only integer division"
  defp isqrt(r, res) do
    if Integer.pow(res, 2) == r do
      res
    else
      res_next = div(res + div(r, res) , 2)
      isqrt(r, res_next)
    end
  end

  defp seed(r) do
    Integer.pow(10, div(r |> Integer.digits |> Enum.count, 2))
  end

end
