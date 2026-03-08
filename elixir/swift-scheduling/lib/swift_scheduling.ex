defmodule SwiftScheduling do
  @doc """
  Convert delivery date descriptions to actual delivery dates, based on when the meeting started.
  """
  @spec delivery_date(NaiveDateTime.t(), String.t()) :: NaiveDateTime.t()
  def delivery_date(md, "NOW"), do: NaiveDateTime.add(md, 2, :hour)
  def delivery_date(md, "ASAP") when md.hour < 13, do: md |> at_hour(17)
  def delivery_date(md, "ASAP"), do: md |> at_hour(13) |> NaiveDateTime.add(1, :day)
  def delivery_date(md, "EOW") do
    case Date.day_of_week(md) do
      x when x < 4 -> md |> at_hour(17) |> NaiveDateTime.add((5 - x), :day)
      x            -> md |> at_hour(20) |> NaiveDateTime.add((7 - x), :day)
    end
  end
  def delivery_date(md, "Q" <> nstr) do
    n = String.to_integer(nstr)
    (if n * 3 > md.month, do: md, else: md |> NaiveDateTime.shift(year: 1))
    |> last_month_of_nth_quarter(n) |> last_work_day_at_8()
  end
  def delivery_date(md, s) do
    n = Integer.parse(s) |> elem(0)
    (if n > md.month, do: md, else: md |> NaiveDateTime.shift(year: 1))
    |> nth_month(n) |> first_work_day_at_8()
  end

  defp at_hour(date, hour), do: date |> NaiveDateTime.to_date() |> NaiveDateTime.new!(Time.new!(hour, 0, 0))
  defp nth_month(date, n), do: %{date | month: n}
  defp last_month_of_nth_quarter(date, n), do: %{date | month: 3*n}
  defp work_days(%{year: yr, month: m}), do: 1..(Calendar.ISO.days_in_month(yr, m)) |> Enum.map(&Date.new!(yr, m, &1)) |> Enum.reject(fn d -> Date.day_of_week(d) >= 6 end)
  defp first_work_day_at_8(date), do: work_days(date) |> hd() |> NaiveDateTime.new!(~T[08:00:00])
  defp last_work_day_at_8(date), do: work_days(date) |> Enum.reverse() |> hd() |> NaiveDateTime.new!(~T[08:00:00])
end
