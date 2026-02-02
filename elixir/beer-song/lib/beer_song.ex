defmodule BeerSong do
  @doc """
  Get a single verse of the beer song
  """
  @spec verse(integer) :: String.t()
  def verse(i) do
    "#{elem(num_of_beers(i), 0)} of beer on the wall, #{elem(num_of_beers(i), 1)} of beer.\n"
    <> case i do
      0 -> "Go to the store and buy some more, 99 bottles of beer on the wall.\n"
      1 -> "Take it down and pass it around, #{elem(num_of_beers(0), 1)} of beer on the wall.\n"
      n -> "Take one down and pass it around, #{elem(num_of_beers(n - 1), 1)} of beer on the wall.\n"
    end
  end

  defp num_of_beers(i) do
    case i do
      0 -> {"No more bottles", "no more bottles"}
      1 -> {"1 bottle", "1 bottle"}
      n -> {"#{n} bottles", "#{n} bottles"}
    end
  end

  @doc """
  Get the entire beer song for a given range of numbers of bottles.
  """
  @spec lyrics(Range.t()) :: String.t()
  def lyrics(), do: lyrics(99..0)
  def lyrics(range) do
    range
    |> Enum.map(&verse/1)
    |> Enum.join("\n")
  end
end
