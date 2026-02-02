defmodule WordSearch do
  @directions [{1, 0},{1, 1},{0, 1},{-1, 1},{-1, 0},{-1, -1},{0, -1},{1, -1}]
  def add_tuples({a1, b1}, {a2, b2}) do
    {a1 + a2, b1 + b2}
  end
  def scalar_mult(k, {a, b}) do
    {k*a, k*b}
  end

  defmodule Location do
    defstruct [:from, :to]

    @type t :: %Location{
            from: %{row: integer, column: integer},
            to: %{row: integer, column: integer}
          }
  end

  @doc """
  Find the start and end positions of words in a grid of letters.
  Row and column positions are 1 indexed.
  """
  @spec search(grid :: String.t(), words :: [String.t()]) :: %{String.t() => nil | Location.t()}
  def search(grid, words) do
    grid_map = grid_to_map(grid)
    words |> Enum.map(&check_word(grid_map, &1)) |> Map.new
  end

  @doc """
  Collects the grid into a map where the key is some character and the value
  is a list of all position index tuples where this character appears.
  """
  @spec grid_to_map(grid :: String.t()) :: %{String.t() => [{integer, integer}]}
  def grid_to_map(grid) do
    grid
    |> String.split("\n", trim: true)
    |> Enum.map(&String.trim/1)
    |> Enum.with_index(1)
    |> Enum.map(fn {s, row} ->
      s
      |> String.codepoints
      |> Enum.with_index(1)
      |> Enum.map(fn {ch, col} -> [{ch, [{row, col}]}] |> Map.new end)
      |> Enum.reduce(%{}, fn m, acc -> Map.merge(acc, m, fn _k, v1, v2 -> v1 ++ v2 end) end)
    end)
    |> Enum.reduce(%{}, fn m, acc -> Map.merge(acc, m, fn _k, v1, v2 -> v1 ++ v2 end) end)
  end

  @doc """
  Enriches the starting positions from the grid_map with all possible 8 directions
  and filters out the start-direction tuples that were successful. Only the first successful is returned
  """
  @spec check_word(grid_map :: %{String.t() => [{integer, integer}]}, word :: String.t()) :: {String.t(), nil | Location.t()}
  def check_word(grid_map, word) do
    if word |> String.codepoints() |> Enum.any?(fn ch -> !Map.has_key?(grid_map, ch) end) do
      {word, nil}
    else
      results =
        Map.fetch!(grid_map, word |> String.first)
        |> Enum.flat_map(fn start -> @directions |> Enum.map(fn direction -> {start, direction} end) end)
        |> Enum.filter(fn {start, direction} -> check_direction_from(start, direction, grid_map, word, 1) end)
      with {:ok, {from, direction}} <- Enum.fetch(results, 0) do
        to = add_tuples(from, scalar_mult(String.length(word) - 1, direction))
        {word, %Location{from: %{row: elem(from, 0), column: elem(from, 1)}, to: %{row: elem(to, 0), column: elem(to, 1)}}}
      else
        :error -> {word, nil}
      end
    end
  end

  @doc """
  Recursively goes through the letters of the word and checks if the necessary calculated position appears in the grid map
  """
  @spec check_direction_from(start :: {integer, integer}, direction :: {integer, integer}, grid_map :: %{String.t() => [{integer, integer}]}, word :: String.t(), index :: integer) :: bool
  def check_direction_from(start, direction, grid_map, word, index) do
    if index == String.length(word) do
      true
    else
      if Map.fetch!(grid_map, word |> String.at(index)) |> Enum.member?(add_tuples(start, scalar_mult(index, direction))) do
        check_direction_from(start, direction, grid_map, word, index + 1)
      else
        false
      end
    end
  end
end
