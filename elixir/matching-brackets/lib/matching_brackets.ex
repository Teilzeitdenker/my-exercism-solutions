defmodule MatchingBrackets do

  @opened ["(", "[", "{"]
  @closed [")", "]", "}"]

  @doc """
  Checks that all the brackets and braces in the string are matched correctly, and nested correctly
  """
  @spec check_brackets(String.t()) :: boolean
  def check_brackets(str) do
    result = str |> String.codepoints() |> Enum.reduce([], &fill_bracket_stack/2)
    if is_nil(result), do: false, else: result |> Enum.count() == 0
  end

  @spec fill_bracket_stack(String.t(), [String.t()]) :: [String.t()]
  defp fill_bracket_stack(_, nil), do: nil
  defp fill_bracket_stack(actual, brackets) do
    cond do
      @opened |> Enum.member?(actual) ->
        [actual | brackets]
      @closed |> Enum.member?(actual) ->
        if brackets |> Enum.empty?() do
          nil
        else
          [last_bracket | rest] = brackets
          actual_index = @closed |> Enum.find_index(fn el -> el == actual end)
          last_index = @opened |> Enum.find_index(fn el -> el == last_bracket end)
          if actual_index == last_index, do: rest, else: nil
        end
      true ->
        brackets
    end
  end
end
