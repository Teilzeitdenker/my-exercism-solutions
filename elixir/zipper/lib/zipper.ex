defmodule Zipper do

  import BinTree

  @type t :: %Zipper{focus: BinTree.t(), parents: [{BinTree.t(), boolean()}]}
  defstruct [:focus, :parents]

  @doc """
  Get a zipper focused on the root node.
  """
  @spec from_tree(BinTree.t(), [{BinTree.t(), boolean()}]) :: Zipper.t()
  def from_tree(bin_tree, new_parents \\ []) do
    %Zipper{focus: bin_tree, parents: new_parents}
  end

  @doc """
  Get the complete tree from a zipper.
  """
  @spec to_tree(Zipper.t()) :: BinTree.t()
  def to_tree(zipper) do
    case zipper.parents do
      [] -> zipper.focus
      _  -> up(zipper) |> to_tree()
    end
  end

  @doc """
  Get the value of the focus node.
  """
  @spec value(Zipper.t()) :: any
  def value(zipper) do
    zipper.focus.value
  end

  @doc """
  Get the left child of the focus node, if any.
  """
  @spec left(Zipper.t()) :: Zipper.t() | nil
  def left(zipper) do
    if is_nil(zipper.focus.left), do: nil, else: from_tree(zipper.focus.left, [{zipper.focus, true}|zipper.parents])
  end

  @doc """
  Get the right child of the focus node, if any.
  """
  @spec right(Zipper.t()) :: Zipper.t() | nil
  def right(zipper) do
    if is_nil(zipper.focus.right), do: nil, else: from_tree(zipper.focus.right, [{zipper.focus, false}|zipper.parents])
  end

  @doc """
  Get the parent of the focus node, if any.
  """
  @spec up(Zipper.t()) :: Zipper.t() | nil
  def up(zipper) do
    if zipper.parents |> Enum.count() == 0 do
      nil
    else
      [{tree, is_left}|rest] = zipper.parents
      new_focus = %BinTree{
        value: tree.value,
        left: if(is_left, do: zipper.focus, else: tree.left),
        right: if(is_left, do: tree.right, else: zipper.focus)
      }
      from_tree(new_focus, rest)
    end
  end

  @doc """
  Set the value of the focus node.
  """
  @spec set_value(Zipper.t(), any) :: Zipper.t()
  def set_value(zipper, value) do
    %{zipper | focus: %{zipper.focus | value: value}}
  end

  @doc """
  Replace the left child tree of the focus node.
  """
  @spec set_left(Zipper.t(), BinTree.t() | nil) :: Zipper.t()
  def set_left(zipper, left) do
    %{zipper | focus: %{zipper.focus | left: left}}
  end

  @doc """
  Replace the right child tree of the focus node.
  """
  @spec set_right(Zipper.t(), BinTree.t() | nil) :: Zipper.t()
  def set_right(zipper, right) do
    %{zipper | focus: %{zipper.focus | right: right}}
  end
end
