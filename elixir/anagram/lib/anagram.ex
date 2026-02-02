defmodule Anagram do
  @doc """
  Returns all candidates that are anagrams of, but not equal to, 'target'.
  """
  defp char_map(letters) do
    letters |> String.downcase() |> String.to_charlist() |> Enum.frequencies()
  end

  defp anagram?(target_map, source) do
    char_map(source) == target_map
  end

  defp different?(target, source) do
    String.downcase(target) != String.downcase(source)
  end

  @spec match(String.t(), [String.t()]) :: [String.t()]
  def match(target, candidates) do
    target_map = char_map(target)
    candidates |> Enum.filter(&different?(target, &1)) |> Enum.filter(&anagram?(target_map, &1))
  end
end
