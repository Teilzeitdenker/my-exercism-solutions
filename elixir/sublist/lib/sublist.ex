defmodule Sublist do
  @doc """
  Returns whether the first list is a sublist or a superlist of the second list
  and if not whether it is equal or unequal to the second list.
  """
  def compare(a, b) do
    case {a |> length, b |> length} do
      {n, n} -> if a === b, do: :equal, else: :unequal
      {m, n} when m < n -> if sublist_of?(a, b), do: :sublist, else: :unequal
      {n, m} when m < n -> if sublist_of?(b, a), do: :superlist, else: :unequal
    end
  end

  defp sublist_of?(a, b) do
    if a == [], do: true, else: b |> Stream.chunk_every(length(a), 1, :discard) |> Enum.any?(&(&1 === a))
  end
end
