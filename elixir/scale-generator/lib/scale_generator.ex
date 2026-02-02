defmodule ScaleGenerator do

  @sharps  ["A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#"]
  @flats   ["A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab"]
  # the successful F# code

  # let sharps = ["A"; "A#"; "B"; "C"; "C#"; "D"; "D#"; "E"; "F"; "F#"; "G"; "G#"] |> List.replicate 2 |> List.concat
  # let flats =  ["A"; "Bb"; "B"; "C"; "Db"; "D"; "Eb"; "E"; "F"; "Gb"; "G"; "Ab"] |> List.replicate 2 |> List.concat
  # let sharpTonics = ["C"; "G"; "D"; "A"; "E"; "B"; "F#"; "a"; "e"; "b"; "f#"; "c#"; "g#"; "d#"]

  # let toInvariant tonic =
  #     if tonic |> String.length = 1 then tonic.ToUpper()
  #     else tonic.[0].ToString().ToUpper() + tonic.[1].ToString()

  # let chromatic tonic =
  #     // hier die ganz kurze Version ohne vorheriges let-binding der scale
  #     if sharpTonics |> List.contains tonic then sharps else flats    // und zack! direkt in die Pipeline damit, quasi wie ein " .. ? .. : .." in C#
  #     |> List.skipWhile ((<>) (toInvariant tonic))
  #     |> List.take 12

  # let interval (intervals: string) tonic =
  #     let filterPattern =
  #         intervals
  #         |> Seq.collect (function
  #             | 'm' -> [true]
  #             | 'M' -> [true; false]
  #             | 'A' -> [true; false; false]
  #             |  _   -> raise <| System.ArgumentException("Invalid interval") )
  #         |> Seq.toList
  #   chromatic tonic
  #   // nimmt man hier <|, dann wird filterPattern als ZWEITER Parameter eingesetzt! Faszinierend!
  #   // Man kann aber natürlich <| auch einfach weglassen und dann eben nach fst filtern und danach auf snd mappen
  #   |> List.zip <| filterPattern
  #   |> List.filter snd
  #   |> List.map fst


  @doc """
  Find the note for a given interval (`step`) in a `scale` after the `tonic`.

  "m": one semitone
  "M": two semitones (full tone)
  "A": augmented second (three semitones)

  Given the `tonic` "D" in the `scale` (C C# D D# E F F# G G# A A# B C), you
  should return the following notes for the given `step`:

  "m": D#
  "M": E
  "A": F
  """
  @spec step(scale :: list(String.t()), tonic :: String.t(), step :: String.t()) :: String.t()
  def step(scale, tonic, step) do
    to_drop = case step do
      "m" -> 1
      "M" -> 2
      "A" -> 3
    end
    scale |> Enum.drop_while(fn el -> el != tonic end) |> Enum.drop(to_drop) |> List.first()
  end

  @doc """
  The chromatic scale is a musical scale with thirteen pitches, each a semitone
  (half-tone) above or below another.

  Notes with a sharp (#) are a semitone higher than the note below them, where
  the next letter note is a full tone except in the case of B and E, which have
  no sharps.

  Generate these notes, starting with the given `tonic` and wrapping back
  around to the note before it, ending with the tonic an octave higher than the
  original. If the `tonic` is lowercase, capitalize it.

  "C" should generate: ~w(C C# D D# E F F# G G# A A# B C)
  """
  @spec chromatic_scale(tonic :: String.t()) :: list(String.t())
  def chromatic_scale(tonic \\ "C") do
    cap_tonic = String.capitalize(tonic)
    @sharps |> Enum.drop_while(fn el -> el != cap_tonic end) |> Enum.take(13)
  end

  @doc """
  Sharp notes can also be considered the flat (b) note of the tone above them,
  so the notes can also be represented as:

  A Bb B C Db D Eb E F Gb G Ab

  Generate these notes, starting with the given `tonic` and wrapping back
  around to the note before it, ending with the tonic an octave higher than the
  original. If the `tonic` is lowercase, capitalize it.

  "C" should generate: ~w(C Db D Eb E F Gb G Ab A Bb B C)
  """
  @spec flat_chromatic_scale(tonic :: String.t()) :: list(String.t())
  def flat_chromatic_scale(tonic \\ "C") do
    cap_tonic = String.capitalize(tonic)
    @flats |> Enum.drop_while(fn el -> el != cap_tonic end) |> Enum.take(13)
  end

  @doc """
  Certain scales will require the use of the flat version, depending on the
  `tonic` (key) that begins them, which is C in the above examples.

  For any of the following tonics, use the flat chromatic scale:

  F Bb Eb Ab Db Gb d g c f bb eb

  For all others, use the regular chromatic scale.
  """
  @spec find_chromatic_scale(tonic :: String.t()) :: list(String.t())
  def find_chromatic_scale(tonic) do
    flat_tonics = ~w[F Bb Eb Ab Db Gb d g c f bb eb]
    if flat_tonics |> Enum.member?(tonic) do
      flat_chromatic_scale(tonic)
    else
      chromatic_scale(tonic)
    end
  end

  @doc """
  The `pattern` string will let you know how many steps to make for the next
  note in the scale.

  For example, a C Major scale will receive the pattern "MMmMMMm", which
  indicates you will start with C, make a full step over C# to D, another over
  D# to E, then a semitone, stepping from E to F (again, E has no sharp). You
  can follow the rest of the pattern to get:

  C D E F G A B C
  """
  @spec scale(tonic :: String.t(), pattern :: String.t()) :: list(String.t())
  def scale(tonic, pattern) do
    first_part =
      find_chromatic_scale(tonic)
      |> Enum.zip(filter_pattern(pattern))
      |> Enum.filter(&elem(&1, 1))
      |> Enum.map(&elem(&1, 0))
    last_note = first_part |> List.first()
    first_part ++ [last_note]
  end

  def filter_pattern(pattern) do
    pattern |> String.codepoints |> Enum.flat_map(fn
      "m" -> [true]
      "M" -> [true, false]
      "A" -> [true, false, false]
    end)
  end
end
