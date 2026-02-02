defmodule Frequency do
  @doc """
  Count letter frequency in parallel.

  Returns a map of characters to frequencies.

  The number of worker processes to use can be set with 'workers'.
  """
  @spec frequency([String.t()], pos_integer) :: map
  def frequency(texts, workers) do
    texts
    |> Task.async_stream(&one_frequency/1, max_concurrency: workers)
    |> Enum.reduce(%{}, fn {:ok, m1}, acc -> Map.merge(m1, acc, fn _k, v1, v2 -> v1 + v2 end) end)
  end

  def one_frequency(text) do
    text
    |> String.replace(~r/[\d | \s | [:punct:]]/, "")
    |> String.downcase
    |> String.graphemes
    |> Enum.frequencies
  end
end
