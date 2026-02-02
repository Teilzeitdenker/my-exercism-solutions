defmodule Triangle do
  @type kind :: :equilateral | :isosceles | :scalene

  @doc """
  Return the kind of triangle of a triangle with 'a', 'b' and 'c' as lengths.
  """
  @spec kind(number, number, number) :: {:ok, kind} | {:error, String.t()}
  def kind(a, b, c) do
    if not sidelengths_positive?(a, b, c) do
      {:error, "all side lengths must be positive"}
    else
      if not inequalities_fulfilled?(a, b, c) do
        {:error, "side lengths violate triangle inequality"}
      else
        if a == b && b == c do
          {:ok, :equilateral}
        else
          if a == b || b == c || a == c do
            {:ok, :isosceles}
          else
            {:ok, :scalene}
          end
        end
      end
    end
  end

  @spec inequalities_fulfilled?(number, number, number) :: boolean()
  defp inequalities_fulfilled?(a, b, c) do
    (a + b > c) && (a + c > b) && (b + c > a)
  end
  @spec sidelengths_positive?(number, number, number) :: boolean()
  defp sidelengths_positive?(a, b, c) do
    (a > 0) && (b > 0) && (c > 0)
  end

end
