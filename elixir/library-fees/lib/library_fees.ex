defmodule LibraryFees do
  def datetime_from_string(string) do
    NaiveDateTime.from_iso8601!(string)
  end

  def before_noon?(datetime) do
    datetime.hour < 12
  end

  def return_date(cd) do
    if before_noon?(cd) do
      bd = NaiveDateTime.add(cd, 60*60*24*28, :second)
      Date.new!(bd.year, bd.month, bd.day)
    else
      bd = NaiveDateTime.add(cd, 60*60*24*29, :second)
      Date.new!(bd.year, bd.month, bd.day)
    end
  end

  def days_late(planned, actual) do
    planned_date = Date.new!(planned.year, planned.month, planned.day)
    case Date.compare(actual, planned_date) do
      :lt -> 0
      :eq -> 0
      :gt -> Date.diff(actual, planned_date)
    end
  end

  def monday?(datetime) do
    Date.day_of_week(datetime) == 1
  end

  def calculate_late_fee(checkout, return, rate) do
    checkout_date = datetime_from_string(checkout)
    return_date = datetime_from_string(return)
    planned_date = return_date(checkout_date)
    num_days = days_late(planned_date, return_date)
    if monday?(return_date) do
      trunc(rate*num_days /2)
    else
      rate*num_days
    end
  end
end
