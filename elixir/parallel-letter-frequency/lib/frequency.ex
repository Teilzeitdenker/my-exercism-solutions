defmodule Frequency do
  import String
  import Enum
  def frequency(texts, workers) do
    task_fn = fn text -> text |> downcase |> graphemes |> filter(&String.match?(&1, ~r/\p{L}/u)) |> frequencies end
    texts |> Task.async_stream(task_fn, max_concurrency: workers)
          |> map(fn {:ok, map} -> map end)
          |> reduce(%{}, &Map.merge(&1, &2, fn _k, v1, v2 -> v1 + v2 end))
  end
end
