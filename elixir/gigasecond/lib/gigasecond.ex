defmodule Gigasecond do
  @doc """
  Calculate a date one billion seconds after an input date.
  """
  @spec from({{pos_integer, pos_integer, pos_integer}, {pos_integer, pos_integer, pos_integer}}) ::
          {{pos_integer, pos_integer, pos_integer}, {pos_integer, pos_integer, pos_integer}}
  def from({{year, month, day}, {hours, minutes, seconds}}) do
    start = NaiveDateTime.from_iso8601!("#{year}-#{month |> pad_zero}-#{day |> pad_zero} #{hours |> pad_zero}:#{minutes |> pad_zero}:#{seconds |> pad_zero}")
    result = NaiveDateTime.add(start, 1_000_000_000)
    {{result.year, result.month, result.day}, {result.hour, result.minute, result.second}}
  end

  @spec pad_zero(pos_integer()) :: String.t()
  defp pad_zero(s) do
    s |> Integer.to_string() |> String.pad_leading(2, "0")
  end
end
