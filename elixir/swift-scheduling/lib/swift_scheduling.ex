defmodule SwiftScheduling do
  @doc """
  Convert delivery date descriptions to actual delivery dates, based on when the meeting started.
  """
  @spec delivery_date(NaiveDateTime.t(), String.t()) :: NaiveDateTime.t()
  def delivery_date(meeting_date, description) do
    case description do
      "NOW" -> NaiveDateTime.add(meeting_date, 2, :hour)
      "ASAP" when meeting_date.hour < 13 -> meeting_date |> at_hour(17)
      "ASAP" -> meeting_date |> at_hour(13) |> NaiveDateTime.add(1, :day)
      "EOW" -> 
        case Date.day_of_week(meeting_date) do
          x when x < 4 -> 
            meeting_date |> at_hour(17) |> NaiveDateTime.add((5 - x), :day)
          x -> 
            meeting_date |> at_hour(20) |> NaiveDateTime.add((7 - x), :day)
        end
      "Q" <> nstr ->
        n = String.to_integer(nstr)
        (if n * 3 > meeting_date.month, do: meeting_date, else: meeting_date |> next_year())
        |> nth_quarter(n) |> last_work_day_at_8()
      s -> 
        n = Integer.parse(s) |> elem(0)
        (if n > meeting_date.month, do: meeting_date, else: meeting_date |> next_year())
        |> nth_month(n) |> first_work_day_at_8()
    end
  end

  defp at_hour(date, hour), do: 
    date |> NaiveDateTime.to_date() |> NaiveDateTime.new!(Time.new!(hour, 0, 0))
  defp next_year(%{year: yr} = date), do: %{date | year: yr + 1}
  defp nth_month(date, n), do: %{date | month: n}
  defp nth_quarter(date, n), do: %{date | month: 3*n}
  defp work_days(%{year: yr, month: m}) do
    1..(Calendar.ISO.days_in_month(yr, m)) 
    |> Enum.map(&Date.new!(yr, m, &1)) 
    |> Enum.reject(fn d -> Date.day_of_week(d) >= 6 end)
  end
  defp first_work_day_at_8(date), do: 
    work_days(date) |> hd() |> NaiveDateTime.new!(~T[08:00:00])
  defp last_work_day_at_8(date), do: 
    work_days(date) |> Enum.reverse() |> hd() |> NaiveDateTime.new!(~T[08:00:00])
end
