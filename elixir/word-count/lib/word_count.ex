defmodule WordCount do
  @doc """
  Count the number of words in the sentence.

  Words are compared case-insensitively.
  """
  @spec count(String.t()) :: map
  def count(sentence) do
    sentence
    |> String.split([" ", ",", ".", ":", "\n", "_", "!", "&", "@", "$", "%", "^"], trim: true)
    |> Enum.map(&String.replace(&1, ~r/^'/, ""))
    |> Enum.map(&String.replace(&1, ~r/'$/, ""))
    |> Enum.frequencies_by(fn word -> word |> String.downcase() end)
  end
end
