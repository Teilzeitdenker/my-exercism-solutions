defmodule Frequency do
  @doc """
  Count letter frequency in parallel.

  Returns a map of characters to frequencies.

  The number of worker processes to use can be set with 'workers'.
  """
  @spec frequency([String.t()], pos_integer) :: map
  def frequency(texts, workers) do
    texts
    |> Task.async_stream(&one_text_frequency/1, max_concurrency: workers)
    |> Enum.reduce(%{}, fn {:ok, map}, acc ->
        Map.merge(map, acc, fn _k, v1, v2 ->
          v1 + v2
        end)
      end)
  end

  def one_text_frequency(text) do
    text
    |> String.downcase
    |> String.graphemes
    |> Enum.filter(&String.match?(&1, ~r/\p{L}/)) # keep only unicode letters
    |> Enum.frequencies
  end
end
