defmodule Isogram do
  @doc """
  Determines if a word or sentence is an isogram
  """
  @spec isogram?(String.t()) :: boolean
  def isogram?(sentence) do
    cleaned = Regex.replace(~r/\W/, sentence, "") |> String.downcase()
    String.length(cleaned) == cleaned |> String.to_charlist() |> Enum.uniq() |> length()
  end
end
