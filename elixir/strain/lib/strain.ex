defmodule Strain do
  @doc """
  Given a `list` of items and a function `fun`, return the list of items where
  `fun` returns true.

  Do not use `Enum.filter`.
  """
  @spec keep(list :: list(any), fun :: (any -> boolean)) :: list(any)
  def keep(list, fun) do
    # do_it(list, fun, [], true)
    for e <- list, fun.(e), do: e
  end

  @doc """
  Given a `list` of items and a function `fun`, return the list of items where
  `fun` returns false.

  Do not use `Enum.reject`.
  """
  @spec discard(list :: list(any), fun :: (any -> boolean)) :: list(any)
  def discard(list, fun) do
    # do_it(list, fun, [], false)
    keep(list, &(not fun.(&1)))
  end

  # defp do_it([], _ , acc, _), do: Enum.reverse(acc)
  # defp do_it([el|rest], fun, acc, keep_mode) do
  #   case (fun.(el) && keep_mode) || (not fun.(el) && not keep_mode) do
  #     true  -> do_it(rest, fun, [el|acc], keep_mode)
  #     false -> do_it(rest, fun, acc, keep_mode)
  #   end
  # end
end
