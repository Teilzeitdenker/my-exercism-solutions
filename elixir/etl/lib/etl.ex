defmodule ETL do
  @doc """
  Transforms an old Scrabble score system to a new one.

  ## Examples

    iex> ETL.transform(%{1 => ["A", "E"], 2 => ["D", "G"]})
    %{"a" => 1, "d" => 2, "e" => 1, "g" => 2}
  """
  @spec transform(map) :: map
  def transform(input) do
    input
    |> Map.to_list()
    |> Enum.map(&score_list_pair_to_tuple_list/1)
    |> Enum.concat()
    |> Enum.into(%{})
  end

  defp score_list_pair_to_tuple_list({score, ls}) do
    ls |> Enum.map(fn c -> {String.downcase(c), score} end)
  end

end
