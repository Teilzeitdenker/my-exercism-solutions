defmodule Proverb do
  @doc """
  Generate a proverb from a list of strings.
  """
  @spec recite(strings :: [String.t()]) :: String.t()
  def recite(strings) do
    case strings do
      []          -> ""
      [word]      -> moral(word)
      [word | _]
        -> ( strings
             |> Enum.chunk_every(2, 1, :discard)
             |> Enum.map(fn [a, b] -> stanza(a, b) end)
             |> Enum.join()
           ) <> moral(word)
    end
  end

  defp moral(word) do
    "And all for the want of a #{word}.\n"
  end

  defp stanza(a, b) do
    "For want of a #{a} the #{b} was lost.\n"
  end
end
