defmodule SaddlePoints do
  @doc """
  Parses a string representation of a matrix
  to a list of rows
  """
  @spec rows(String.t()) :: [[integer]]
  def rows(""), do: []
  def rows(str) do
    str |> String.split("\n") |> Enum.map(fn substr -> substr |> String.split(" ") |> Enum.map(fn s -> elem(Integer.parse(s), 0)  end) end)
  end

  @doc """
  Parses a string representation of a matrix
  to a list of columns
  """
  @spec columns(String.t()) :: [[integer]]
  def columns(""), do: []
  def columns(str) do
    matrix = rows(str)
    Enum.zip_reduce(matrix, [], fn elements, acc -> acc ++ [elements] end)
  end

  @doc """
  Calculates all the saddle points from a string
  representation of a matrix
  """
  @spec saddle_points(String.t()) :: [{integer, integer}]
  def saddle_points(""), do: []
  def saddle_points(str) do
    rs = rows(str)
    cs = columns(str)
    for i <- 0..(length(rs) - 1),
        j <- 0..(length(cs) - 1),
        saddle_point?(rs |> Enum.at(i) |> Enum.at(j), rs |> Enum.at(i), cs |> Enum.at(j)),
        do: {i + 1, j + 1}
  end

  @spec saddle_point?(integer(), [integer()], [integer()]) :: boolean()
  defp saddle_point?(num, row, column) do
    row |> Enum.all?(fn r -> r <= num end) && column |> Enum.all?(fn c -> c >= num end)
  end

end
