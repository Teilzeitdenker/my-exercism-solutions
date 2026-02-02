defmodule Wordy do
  def answer(question), do: question |> String.split("\s", trim: true) |> loop()
  defp int(x), do: x |> String.trim_trailing("?") |> String.to_integer
  defp loop(["What", "is", fst_num | tail]),    do: loop([int(fst_num) | tail])
  defp loop([x]) when is_number(x),             do: x
  defp loop([x, "plus", y | tail]),             do: loop([x + int(y) | tail])
  defp loop([x, "minus", y | tail]),            do: loop([x - int(y) | tail])
  defp loop([x, "multiplied", "by", y | tail]), do: loop([x * int(y) | tail])
  defp loop([x, "divided", "by", y | tail]),    do: loop([div(x, int(y)) | tail])
  defp loop(_),                                 do: raise ArgumentError
end
