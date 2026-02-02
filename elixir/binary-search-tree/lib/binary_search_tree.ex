defmodule BinarySearchTree do
  @type bst_node :: %{data: any, left: bst_node | nil, right: bst_node | nil}
  def new(data), do: %{data: data, left: nil, right: nil}
  def insert(nil, data), do: new(data)
  def insert(tree, data) when data <= tree.data, do: %{tree | left: insert(tree.left, data)}
  def insert(tree, data), do: %{tree | right: insert(tree.right, data)}
  def in_order(nil), do: []
  def in_order(tree), do: in_order(tree.left) ++ [tree.data | in_order(tree.right)]
end
