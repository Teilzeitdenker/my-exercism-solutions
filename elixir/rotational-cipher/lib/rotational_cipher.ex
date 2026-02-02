defmodule RotationalCipher do
  @doc """
  Given a plaintext and amount to shift by, return a rotated string.

  Example:
  iex> RotationalCipher.rotate("Attack at dawn", 13)
  "Nggnpx ng qnja"
  """
  @spec rotate(text :: String.t(), shift :: integer) :: String.t()
  def rotate(text, shift) do
    text |> String.to_charlist |> Enum.map(fn c ->
      cond do
        c >= ?a and c <= ?z -> shift_by(c, shift, false)
        c >= ?A and c <= ?Z -> shift_by(c, shift, true)
        true -> c
      end
    end)
    |> to_string()
  end

  @spec shift_by(char(), integer(), bool()) :: char()
  defp shift_by(c, shift, upper) do
    zero = if upper, do: ?A, else: ?a
     rem((c - zero) + shift, 26) + zero
  end
end
