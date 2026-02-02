defmodule School do
  @moduledoc """
  Simulate students in a school.

  Each student is in a grade.
  """

  @type school :: %{integer() => [String.t()]}

  @doc """
  Create a new, empty school.
  """
  @spec new() :: school
  def new() do
    %{}
  end

  @doc """
  Add a student to a particular grade in school.
  """
  @spec add(school, String.t(), integer) :: {:ok | :error, school}
  def add(school, name, grade) when is_binary(name) and is_integer(grade) do
    cond do
      roster(school) |> Enum.member?(name)
        -> {:error, school}
      school |> Map.has_key?(grade)
        -> {:ok, %{school | grade => [name | school[grade]] |> Enum.sort()}}
      true
        -> {:ok, Map.put(school, grade, [name])}
    end
  end
  def add(school, _, _), do: {:error, school}

  @doc """
  Return the names of the students in a particular grade, sorted alphabetically.
  """
  @spec grade(school, integer) :: [String.t()]
  def grade(school, grade) do
    if  is_nil(school[grade]), do: [], else: school[grade]
  end

  @doc """
  Return the names of all the students in the school sorted by grade and name.
  """
  @spec roster(school) :: [String.t()]
  def roster(school) do
    (for n <- Map.keys(school) |> Enum.sort(), do: school[n]) |> Enum.concat()
  end
end
