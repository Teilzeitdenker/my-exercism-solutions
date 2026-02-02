defmodule Satellite do
  @typedoc """
  A tree, which can be empty, or made from a left branch, a node and a right branch
  """
  @type tree :: {} | {tree, any, tree}

  @doc """
  Build a tree from the elements given in a pre-order and in-order style
  """
  @spec build_tree(preorder :: [any], inorder :: [any]) :: {:ok, tree} | {:error, String.t()}
  def build_tree(preorder, inorder) do
    cond do
      preorder |> Enum.count() != inorder |> Enum.count() ->
        {:error, "traversals must have the same length"}
      preorder |> Enum.sort()  != inorder |> Enum.sort() ->
        {:error, "traversals must have the same elements"}
      preorder |> Enum.uniq() |> Enum.count() != preorder |> Enum.count() or preorder |> Enum.uniq() |> Enum.count() != preorder |> Enum.count() ->
        {:error, "traversals must contain unique items"}
      true -> {:ok, do_build(preorder, inorder)}
    end
  end

  @spec do_build(preorder :: [any], inorder :: [any]) :: tree
  defp do_build([], []), do: {}
  defp do_build([a], [b]) when a == b, do: {{}, a, {}}
  defp do_build([root | preorder], inorder) do
    {fst_inorder, [_el | snd_inorder]} = inorder |> Enum.split_while(fn item -> item != root end)
    num_to_take = fst_inorder |> Enum.count()
    {do_build(preorder |> Enum.take(num_to_take), fst_inorder), root, do_build(preorder |> Enum.drop(num_to_take), snd_inorder)}
  end
end
