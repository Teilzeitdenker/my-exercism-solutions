defmodule BinarySearchTree do
  @type bst_node :: %{data: any, left: bst_node | nil, right: bst_node | nil}

  @doc """
  Create a new Binary Search Tree with root's value as the given 'data'
  """
  @spec new(any) :: bst_node
  def new(data) do
    %{data: data, left: nil, right: nil}
  end

  @doc """
  Creates and inserts a node with its value as 'data' into the tree.
  """
  @spec insert(bst_node, any) :: bst_node
  def insert(tree, data) do
    if data <= tree.data do
      if tree.left == nil do
        %{tree | left: new(data)}
      else
        %{tree | left: insert(tree.left, data)}
      end
    else
      if tree.right == nil do
        %{tree | right: new(data)}
      else
        %{tree | right: insert(tree.right, data)}
      end
    end
  end

  @doc """
  Traverses the Binary Search Tree in order and returns a list of each node's data.
  """
  @spec in_order(bst_node) :: [any]
  def in_order(tree) do
    if tree == nil do
      []
    else
      in_order(tree.left) ++ [tree.data] ++ in_order(tree.right)
    end
  end
end
