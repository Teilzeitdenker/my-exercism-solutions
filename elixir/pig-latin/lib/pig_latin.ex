defmodule PigLatin do
  @rule1 ~r/^([aeiou]|x[^aeiou]|y[^aeiou])/
  @rule2_4 ~r/^([^aeiou][^aeiouy]*)(.*)$/
  @rule3 ~r/^([^aeiou]*qu)(.*)$/
  @doc """
  Given a `phrase`, translate it a word at a time to Pig Latin.
  """
  @spec translate(phrase :: String.t()) :: String.t()
  def translate(phrase) do
    if String.contains?(phrase, " ") do
      Enum.join( phrase |> String.split |> Enum.map(&translate/1), " ")
    else
      cond do
        Regex.match?(@rule1, phrase) -> phrase <> "ay"
        Regex.match?(@rule3, phrase) ->
          [consclust|[rest|_]] = Regex.run(@rule3, phrase) |> Enum.drop(1) |> Enum.to_list()
          rest <> consclust <> "ay"
        Regex.match?(@rule2_4, phrase) ->
          [consclust|[rest|_]] = Regex.run(@rule2_4, phrase) |> Enum.drop(1) |> Enum.to_list()
          rest <> consclust <> "ay"
        true -> phrase
      end
    end
  end
end
