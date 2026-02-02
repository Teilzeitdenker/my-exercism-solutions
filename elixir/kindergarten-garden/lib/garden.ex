defmodule Garden do
  @children [
    :alice,
    :bob,
    :charlie,
    :david,
    :eve,
    :fred,
    :ginny,
    :harriet,
    :ileana,
    :joseph,
    :kincaid,
    :larry
  ]
  @doc """
    Accepts a string representing the arrangement of cups on a windowsill and a
    list with names of students in the class. The student names list does not
    have to be in alphabetical order.

    It decodes that string into the various gardens for each student and returns
    that information in a map.
  """

  @spec info(String.t(), list) :: map
  def info(info_string, student_names \\ @children) do
    for { name, idx } <- student_names |> Enum.sort() |> Enum.with_index(), into: %{} do
      {name,
        info_string
        |> String.split("\n")
        |> Enum.map(&String.to_charlist/1)
        |> Enum.flat_map(fn line -> line |> Enum.drop(2*idx) |> Enum.take(2) end)
        |> Enum.map(&char_to_plant/1)
        |> List.to_tuple()
      }
    end
  end

  @spec char_to_plant(char()) :: atom()
  defp char_to_plant(c) do
    case c do
      ?V -> :violets
      ?G -> :grass
      ?R -> :radishes
      ?C -> :clover
      _  -> raise "no such plant!"
    end
  end
end
