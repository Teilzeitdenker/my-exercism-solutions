defmodule Say do
  @ones      ~w(zero one two three four five six seven eight nine)
  @to_twenty ~w(ten eleven twelve thirteen fourteen fifteen sixteen seventeen eighteen nineteen)
  @tens      ~w(none ten twenty thirty forty fifty sixty seventy eighty ninety)

  @doc """
  Translate a positive integer into English.
  """
  @spec in_english(integer) :: {atom, String.t()}
  def in_english(n) when n < 0 or n >= 1_000_000_000_000, do: {:error, "number is out of range"}
  def in_english(0), do: {:ok, "zero"}
  def in_english(n) do
    {:ok, loop(n, [])}
  end

  defp loop(0, acc), do: acc |> Enum.join(" ")
  defp loop(n, acc) when n < 1000, do: loop(0, acc ++ get_part(n))
  defp loop(n, acc) when n < 1_000_000, do: loop(rem(n, 1_000), acc ++ get_part(div(n, 1_000)) ++ ["thousand"])
  defp loop(n, acc) when n < 1_000_000_000, do: loop(rem(n, 1_000_000), acc ++ get_part(div(n, 1_000_000)) ++ ["million"])
  defp loop(n, acc), do: loop(rem(n, 1_000_000_000), acc ++ get_part(div(n, 1_000_000_000)) ++ ["billion"])

  defp get_part(0), do: []
  defp get_part(n) when n < 10, do: [Enum.at(@ones, n)]
  defp get_part(n) when n < 20, do: [Enum.at(@to_twenty, n - 10)]
  defp get_part(n) when n < 100 do
    case rem(n, 10) do
      0 -> [Enum.at(@tens, div(n, 10))]
      r -> ["#{Enum.at(@tens, div(n, 10))}-#{Enum.at(@ones, r)}"]
    end
  end
  defp get_part(n), do: ["#{Enum.at(@ones, div(n, 100))} hundred"] ++ get_part(rem(n, 100))
end
