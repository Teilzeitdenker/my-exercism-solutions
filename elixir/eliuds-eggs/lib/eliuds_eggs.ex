use Bitwise
defmodule EliudsEggs do
  @doc """
  Given the number, count the number of eggs.
  """
  @spec egg_count(number :: integer()) :: non_neg_integer()
  def egg_count(number), do: do_count(number, 0)
  defp do_count(0, cnt), do: cnt
  defp do_count(number, cnt), do: (if (number &&& 1) == 1, do: do_count(number >>> 1, cnt + 1), else: do_count(number >>> 1, cnt))
end
