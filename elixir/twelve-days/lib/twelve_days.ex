defmodule TwelveDays do
  @numbers ["a", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve"]
  @ordinals ["first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "ninth", "tenth", "eleventh", "twelfth"]
  @gifts ["Partridge in a Pear Tree", "Turtle Doves", "French Hens", "Calling Birds", "Gold Rings", "Geese-a-Laying",
    "Swans-a-Swimming", "Maids-a-Milking", "Ladies Dancing", "Lords-a-Leaping", "Pipers Piping", "Drummers Drumming"]

  @doc """
  Given a `number`, return the song's verse for that specific day, including
  all gifts for previous days in the same line.
  """
  @spec verse(number :: integer) :: String.t()
  def verse(number) do
    ordinal = @ordinals |> Enum.at(number - 1)
    first_gift = "#{Enum.at(@numbers, 0)} #{Enum.at(@gifts, 0)}."
    gifts =
      if number == 1 do
        first_gift
      else
        other_gifts =
          for i <- Enum.reverse(1..number-1) do
            amount = @numbers |> Enum.at(i)
            gift = @gifts |> Enum.at(i)
            "#{amount} #{gift}"
          end
        Enum.join(other_gifts, ", ")  <> ", and #{first_gift}"
      end
    "On the #{ordinal} day of Christmas my true love gave to me: #{gifts}"
  end

  @doc """
  Given a `starting_verse` and an `ending_verse`, return the verses for each
  included day, one per line.
  """
  @spec verses(starting_verse :: integer, ending_verse :: integer) :: String.t()
  def verses(starting_verse, ending_verse) do
    starting_verse..ending_verse |> Enum.map(&verse/1) |> Enum.join("\n")
  end

  @doc """
  Sing all 12 verses, in order, one verse per line.
  """
  @spec sing() :: String.t()
  def sing do
    verses(1, 12)
  end
end
