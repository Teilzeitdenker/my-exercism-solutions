defmodule Meetup do
  @moduledoc """
  Calculate meetup dates.
  """

  @type weekday ::
          :monday
          | :tuesday
          | :wednesday
          | :thursday
          | :friday
          | :saturday
          | :sunday

  @type schedule :: :first | :second | :third | :fourth | :last | :teenth

  @doc """
  Calculate a meetup date.

  The schedule is in which week (1..4, last or "teenth") the meetup date should
  fall.
  """
  @spec meetup(pos_integer, pos_integer, weekday, schedule) :: Date.t()
  def meetup(year, month, weekday, schedule) do
    start =
      case schedule do
        :first  -> 1
        :second -> 8
        :third  -> 15
        :fourth -> 22
        :teenth -> 13
        :last   -> 0
      end
    day_of_week =
      case weekday do
        :monday    -> 1
        :tuesday   -> 2
        :wednesday -> 3
        :thursday  -> 4
        :friday    -> 5
        :saturday  -> 6
        :sunday    -> 7
      end
    i = if start > 0 do
      start
    else
      Date.days_in_month(Date.new!(year, month, 1)) - 6
    end
    dt = Date.new!(year, month, i)
    days_to_add = rem(day_of_week - Date.day_of_week(dt) + 7, 7)
    dt |> Date.add(days_to_add)
  end

end
