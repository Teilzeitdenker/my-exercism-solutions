defmodule Matrix do
  defstruct matrix: nil

  @doc """
  Convert an `input` string, with rows separated by newlines and values
  separated by single spaces, into a `Matrix` struct.
  """
  @spec from_string(input :: String.t()) :: %Matrix{}
  def from_string(input) do
    matrix =
      for line <- input |> String.split("\n") do
        for n <- line |> String.split(" ") do
          n |> String.to_integer()
        end
      end
    %Matrix{matrix: matrix}
  end

  @doc """
  Write the `matrix` out as a string, with rows separated by newlines and
  values separated by single spaces.
  """
  @spec to_string(matrix :: %Matrix{}) :: String.t()
  def to_string(matrix) do
    matrix.matrix |> Enum.map(fn v -> v |> Enum.map(fn n -> Kernel.to_string(n) end) |> Enum.join(" ") end) |> Enum.join("\n")
  end

  @doc """
  Given a `matrix`, return its rows as a list of lists of integers.
  """
  @spec rows(matrix :: %Matrix{}) :: list(list(integer))
  def rows(matrix), do: matrix.matrix

  @doc """
  Given a `matrix` and `index`, return the row at `index`.
  """
  @spec row(matrix :: %Matrix{}, index :: integer) :: list(integer)
  def row(matrix, index), do: matrix.matrix |> Enum.at(index - 1)

  @doc """
  Given a `matrix`, return its columns as a list of lists of integers.
  """
  @spec columns(matrix :: %Matrix{}) :: list(list(integer))
  def columns(matrix) do
    num_cols = matrix.matrix |> Enum.at(0) |> Enum.count()
    num_rows = matrix.matrix |> Enum.count()
    for j <- 0..num_cols-1 do
      for i <- 0..num_rows-1 do
        matrix.matrix |> Enum.at(i) |> Enum.at(j)
      end
    end
  end

  @doc """
  Given a `matrix` and `index`, return the column at `index`.
  """
  @spec column(matrix :: %Matrix{}, index :: integer) :: list(integer)
  def column(matrix, index) do
    num_rows = matrix.matrix |> Enum.count()
    for i <- 0..num_rows-1 do
      matrix.matrix |> Enum.at(i) |> Enum.at(index - 1)
    end
  end
end
