defmodule FoodChain do
  @tiere ~w(fly spider bird cat dog goat cow horse)
  @ausrufe [
    "I don't know why she swallowed the fly. Perhaps she'll die.",
    "It wriggled and jiggled and tickled inside her.",
    "How absurd to swallow a bird!",
    "Imagine that, to swallow a cat!",
    "What a hog, to swallow a dog!",
    "Just opened her throat and swallowed a goat!",
    "I don't know how she swallowed a cow!",
    "She's dead, of course!"
  ]

  defp sonderfall(n) do
    if @tiere |> Enum.at(n) == "spider", do: " that wriggled and jiggled and tickled inside her", else: ""
  end

  defp verse(n) do
    ersteZeilen = ["I know an old lady who swallowed a #{@tiere |> Enum.at(n-1)}.", @ausrufe |> Enum.at(n-1)]
    if n == 8 or n == 1 do
      (ersteZeilen |> Enum.join("\n")) <> "\n"
    else
      mittlereZeilen =
        for i <- Enum.reverse(1..n-1) do
          "She swallowed the #{@tiere |> Enum.at(i)} to catch the #{@tiere |> Enum.at(i-1)}#{sonderfall(i-1)}."
        end
      letzteZeile = [@ausrufe |> Enum.at(0)]
      (ersteZeilen ++ mittlereZeilen ++ letzteZeile |> Enum.join("\n")) <> "\n"
    end
  end

  @doc """
  Generate consecutive verses of the song 'I Know an Old Lady Who Swallowed a Fly'.
  """
  @spec recite(start :: integer, stop :: integer) :: String.t()
  def recite(start, stop) do
    start..stop |> Enum.map(&verse/1) |> Enum.join("\n")
  end
end
