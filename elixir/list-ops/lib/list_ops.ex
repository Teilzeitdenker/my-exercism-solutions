defmodule ListOps do
  # Please don't use any external modules (especially List or Enum) in your
  # implementation. The point of this exercise is to create these basic
  # functions yourself. You may use basic Kernel functions (like `Kernel.+/2`
  # for adding numbers), but please do not use Kernel functions for Lists like
  # `++`, `--`, `hd`, `tl`, `in`, and `length`.

  @spec count(list) :: non_neg_integer
  def count([]), do: 0
  def count([_|t]), do: 1 + count(t)

  @spec reverse(list) :: list
  def reverse(l) do
    reverse_acc([], l)
  end

  defp reverse_acc(acc, []), do: acc
  defp reverse_acc(acc, [h|t]), do: reverse_acc([h|acc], t)

  @spec map(list, (any -> any)) :: list
  def map(l, f) do
    reverse(map_acc([], l, f))
  end

  defp map_acc(acc, [], _), do: acc
  defp map_acc(acc, [h|t], f), do: map_acc([f.(h)|acc], t, f)

  @spec filter(list, (any -> as_boolean(term))) :: list
  def filter(l, f) do
    reverse(filter_acc([], l, f))
  end

  defp filter_acc(acc, [], _), do: acc
  defp filter_acc(acc, [h|t], f), do: filter_acc (if f.(h), do: [h|acc], else: acc), t, f

  @type acc :: any
  @spec foldl(list, acc, (any, acc -> acc)) :: acc
  def foldl([], acc, _), do: acc
  def foldl([h|t], acc, f) do
    foldl(t, f.(h, acc), f)
  end

  @spec foldr(list, acc, (any, acc -> acc)) :: acc
  def foldr(l, acc, f) do
    foldl(reverse(l), acc, f)
  end

  @spec append(list, list) :: list
  def append(a, b), do: reverse(append_rev(reverse(a), b))

  defp append_rev(a, []), do: a
  defp append_rev(a, [h|t]), do: append_rev([h|a], t)

  @spec concat([[any]]) :: [any]
  def concat([]), do: []
  def concat(ll) do
    concat_acc([], ll)
  end

  defp concat_acc(acc, []), do: reverse(acc)
  defp concat_acc(acc, [h|t]), do: concat_acc(append_rev(acc, h), t)
end
