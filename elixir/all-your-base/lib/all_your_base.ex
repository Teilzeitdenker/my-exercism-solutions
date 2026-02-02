defmodule AllYourBase do
  @doc """
  Given a number in input base, represented as a sequence of digits, converts it to output base,
  or returns an error tuple if either of the bases are less than 2
  """

  @spec convert(list, integer, integer) :: {:ok, list} | {:error, String.t()}
  def convert(digits, input_base, output_base) do
    cond do
      output_base <= 1 ->
        {:error, "output base must be >= 2"}
      input_base <= 1 ->
        {:error, "input base must be >= 2"}
      Enum.any?(digits, fn el -> el < 0 || el >= input_base end) ->
        {:error, "all digits must be >= 0 and < input base"}
      Enum.sum(digits) == 0 ->
        {:ok, [0]}
      true ->
        number = digits |> Enum.reverse() |> Enum.with_index() |> Enum.map(fn t -> elem(t, 0) * (input_base**elem(t, 1)) end) |> Enum.sum()
        {:ok, rebase(number, output_base)}
    end
  end

  defp rebase(n, base) do
    if n < base do
      [n]
    else
      rebase(div(n, base), base) ++ [rem(n, base)]
    end
  end


end
