defmodule FreelancerRates do
  def daily_rate(hourly_rate) do
    8.0*hourly_rate
  end

  def apply_discount(before_discount, discount) do
    before_discount*(100.0 - discount)/100
  end

  def monthly_rate(hourly_rate, discount) do
    ceil(apply_discount(daily_rate(hourly_rate)*22.0, discount))
  end

  def days_in_budget(budget, hourly_rate, discount) do
    floor((10 * budget) / apply_discount(daily_rate(hourly_rate), discount)) / 10
  end
end
