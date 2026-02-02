defmodule Anagram do
  @doc """
  Returns all candidates that are anagrams of, but not equal to, 'target'.
  """
  def char_map(letters) do
    letters |> String.downcase() |> String.to_charlist() |> Enum.frequencies()
  end

  def is_anagram?(target_map, source) do
    char_map(source) == target_map
  end

  def is_different?(target, source) do
    String.downcase(target) != String.downcase(source)
  end

  @spec match(String.t(), [String.t()]) :: [String.t()]
  def match(target, candidates) do
    target_map = char_map(target)
    candidates |> Enum.filter(&is_different?(target, &1)) |> Enum.filter(&is_anagram?(target_map, &1))
  end
end
