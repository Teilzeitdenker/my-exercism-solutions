defmodule Darts do
  @type position :: {number, number}

  @doc """
  Calculate the score of a single dart hitting a target
  """
  @spec score(position) :: integer
  def score({x, y}) do
    radius = (:math.pow(x, 2) + :math.pow(y, 2)) |> :math.sqrt()
    cond do
      radius <= 1.0  -> 10
      radius <= 5.0  -> 5
      radius <= 10.0 -> 1
      true           -> 0
    end
  end
end
