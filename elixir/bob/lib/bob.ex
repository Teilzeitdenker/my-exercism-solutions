defmodule Bob do
  @spec hey(String.t()) :: String.t()
  def hey(input) do
    s = input |> String.trim()
    if s == "" do
      "Fine. Be that way!"
    else
      question = question?(s)
      yelled = yelled?(s)
      case {question, yelled} do
        {true, true} -> "Calm down, I know what I'm doing!"
        {true, _   } -> "Sure."
        { _  , true} -> "Whoa, chill out!"
        _            -> "Whatever."
      end
    end
  end

  defp question?(s) do
    if s == "" do
      false
    else
      String.ends_with?(s, "?")
    end
  end

  defp contains_letters?(s) do
    Regex.match?(~r/[[:alpha:]]/, s)
  end

  defp yelled?(s) do
    if contains_letters?(s) do
      s |> String.upcase() == s
    else
      false
    end
  end
end
