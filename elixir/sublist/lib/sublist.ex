defmodule Sublist do
  @doc """
  Returns whether the first list is a sublist or a superlist of the second list
  and if not whether it is equal or unequal to the second list.
  """
  def compare(a, b) do
    case {a |> length, b |> length} do
      {0, 0} -> :equal
      {0, _} -> :sublist
      {_, 0} -> :superlist
      {n, n} ->
        case a === b do
          true -> :equal
          _    -> :unequal
        end
      {m, n} when m < n ->
        case b |> Enum.chunk_every(m, 1, :discard) |> Enum.any?(&(&1 === a)) do
          true -> :sublist
          _    -> :unequal
        end
      {n, m} when m < n ->
        case a |> Enum.chunk_every(m, 1, :discard) |> Enum.any?(&(&1 === b)) do
          true -> :superlist
          _    -> :unequal
        end
    end
  end
end
