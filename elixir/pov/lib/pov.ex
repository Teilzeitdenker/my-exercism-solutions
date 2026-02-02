defmodule Pov do
  @typedoc """
  A tree, which is made of a node with several branches
  """
  @type tree :: {any, [tree]}

  # see the solution in F#, in Elixir additional types
  # have to be defined in their own modules
  defmodule Path do
    defstruct [:value, :lft, :rgt] # left_of_focus and right_of_focus
    @type t :: %Path{value: any, lft: [Pov.tree()], rgt: [Pov.tree()]}
  end
  defmodule Zipper do
    defstruct [:focus, :paths]
    @type t :: %Zipper{focus: Pov.tree(), paths: [Path.t()]}
  end

  @doc """
  Reparent a tree on a selected node.
  """
  @spec from_pov(tree :: tree, node :: any) :: {:ok, tree} | {:error, atom}
  def from_pov(tree, node) do
    case zipper_from(tree) |> find(node) do
      {:ok, zipper} -> {:ok,    reparent(zipper)}
      _             -> {:error, :nonexistent_target}
    end
  end

  @doc """
  Finds a path between two nodes
  """
  @spec path_between(tree :: tree, from :: any, to :: any) :: {:ok, [any]} | {:error, atom}
  def path_between(tree, from, to) do
    case zipper_from(tree) |> find(from) do
      {:ok, zipper} ->
        case zipper_from(reparent(zipper)) |> find(to) do
          {:ok, nxt_zipper} -> {:ok,    trace_from_zipper(nxt_zipper)}
          _                 -> {:error, :nonexistent_destination}
        end
      _            -> {:error, :nonexistent_source}
    end
  end
  # private helper methods
  defp zipper_from(tree), do: %Zipper{focus: tree, paths: []}
  defp path_value(%Path{value: v}), do: v
  defp trace_from_zipper(%Zipper{focus: {v, _}, paths: paths}) do
    [v | Enum.map(paths,&path_value/1)] |> Enum.reverse
  end
  # set cursor to first child (if existing) and prepend according path to paths
  defp down(%Zipper{focus: {v, [ch | chs]}, paths: paths}) do
    %Zipper{focus: ch, paths: [%Path{value: v, rgt: chs, lft: []} | paths]}
  end
  defp down(_), do: nil # Elixir way of getting a None
  # set cursor to next right child of the first path in paths list (if existing) and update this path
  defp right(%Zipper{focus: tree, paths: [%Path{lft: lft, rgt: [rgt | rgts]} = path | ps]}) do
    %Zipper{focus: rgt, paths: [%Path{path | lft: [tree | lft], rgt: rgts} | ps]}
  end
  defp right(_), do: nil
  defp find(%Zipper{focus: {node, _}} = zipper, node), do: {:ok, zipper}
  defp find(nil, _), do: nil
  defp find(%Zipper{} = zipper, node) do
    case down(zipper) |> find(node) do # recurse at next level and match
      {:ok, z} -> {:ok, z}
      _        -> right(zipper) |> find(node)
    end
  end
  defp reparent(%Zipper{focus: tree, paths: []}), do: tree
  defp reparent(%Zipper{focus: {focus_v, chs}, paths: [%Path{value: path_v, lft: lft, rgt: rgt} | ps]}) do
    parent_perspective = reparent(%Zipper{focus: {path_v, lft ++ rgt}, paths: ps})
    {focus_v, [parent_perspective | chs]} # add the parent perspective to the children
  end
end
