defmodule Frequency do
  @spec frequency([String.t()], pos_integer) :: map
  def frequency(texts, workers) do
    texts
    |> Task.async_stream(&one_text_frequency/1, max_concurrency: workers)
    |> Enum.map(fn {:ok, map} -> map end)
    |> Enum.reduce(%{}, &Map.merge(&1, &2, fn _k, v1, v2 -> v1 + v2 end))
  end
  defp one_text_frequency(text) do
    text |> String.downcase |> String.graphemes |> Enum.filter(&String.match?(&1, ~r/\p{L}/u)) |> Enum.frequencies
  end
end
