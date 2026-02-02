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
      preorder |> Enum.count() != inorder |> Enum.count() -> {:error, "traversals must have the same length"}
      preorder |> Enum.sort()  != inorder |> Enum.sort() ->  {:error, "traversals must have the same elements"}
      preorder |> Enum.uniq() != preorder or preorder |> Enum.uniq() != preorder -> {:error, "traversals must contain unique items"}
      true -> {:ok, do_build(preorder, inorder)}
    end
  end

  @spec do_build(preorder :: [any], inorder :: [any]) :: tree
  defp do_build([], []), do: {}
  defp do_build([a], [b]) when a == b, do: {{}, a, {}}
  defp do_build([root | preorder], inorder) do
    {fst_inorder, [_root | snd_inorder]} = inorder |> Enum.split_while(&(&1 != root))
    {fst_preorder, snd_preorder} = preorder |> Enum.split(fst_inorder |> Enum.count)
    {do_build(fst_preorder, fst_inorder), root, do_build(snd_preorder, snd_inorder)}
  end
end
