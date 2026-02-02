defmodule Rectangles do
  @horizontal_conn ["-", "+"]
  @vertical_conn ["|", "+"]

  @doc """
  Count the number of ASCII rectangles.
  """
  @spec count(input :: String.t()) :: integer
  def count(input) do
    lines_of_codepoints =
      input
      |> String.split("\n", trim: true)
      |> Enum.map(&String.codepoints/1)

    corner_pairs =
      lines_of_codepoints
      |> Enum.map(fn line ->
        line
        |> Enum.with_index
        |> Enum.filter(&(elem(&1,0) == "+"))
        |> Enum.map(&elem(&1,1))
        |> get_corner_pairs()
      end)
      |> Enum.with_index

    # Go through all these pairs, for each pair check every later line (higher line_index) for the same pair,
    # then use this list of candidates for line2 with the check_rectangle function and count successes
    corner_pairs
    |> Enum.map(fn {pairs, line1} ->
      pairs
      |> Enum.map(fn pair = {col1, col2} ->
        get_candidates_for_line2(pair, corner_pairs, line1)
        |> Enum.count(&check_rectangle(lines_of_codepoints, line1, &1, col1, col2))
      end)
      |> Enum.sum()
    end)
    |> Enum.sum()
  end

  defp get_corner_pairs(corner_list) do
    # Get every pair of corners once! Since the  ordered, can use drop_while
    # in F# there is a nice function in the sequence module for this: Seq.allPairs s
    corner_list
    |> Enum.flat_map(fn x -> corner_list |> Enum.drop_while(fn y -> y <= x end) |> Enum.map(fn y -> {x, y} end)  end)
  end

  defp get_candidates_for_line2(pair, corner_pairs, from_line_index) do
    corner_pairs
      |> Enum.drop(from_line_index + 1)
      # is this pair under the corner pairs of the later line?
      |> Enum.filter(&Enum.member?(elem(&1, 0), pair))
      # only keep the line index then
      |> Enum.map(&elem(&1, 1))
  end

  defp check_rectangle(lines_of_codepoints, line1, line2, col1, col2) do
    transpose = lines_of_codepoints |> Enum.zip() |> Enum.map(&Tuple.to_list/1)
    # check horizontal connection
    lines_of_codepoints |> Enum.at(line1) |> Enum.slice(col1..col2) |> Enum.all?(&Enum.member?(@horizontal_conn, &1)) and
    lines_of_codepoints |> Enum.at(line2) |> Enum.slice(col1..col2) |> Enum.all?(&Enum.member?(@horizontal_conn, &1)) and
    # and vertical connection
    transpose |> Enum.at(col1) |> Enum.slice(line1..line2) |> Enum.all?(&Enum.member?(@vertical_conn, &1)) and
    transpose |> Enum.at(col2) |> Enum.slice(line1..line2) |> Enum.all?(&Enum.member?(@vertical_conn, &1))
  end

end
