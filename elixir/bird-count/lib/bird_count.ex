defmodule BirdCount do
  def today([]) do
    nil
  end
  def today([h | _]) do
    h
  end

  def increment_day_count([]) do
    [1]
  end
  def increment_day_count([h | r]) do
    [h + 1 | r]
  end

  def has_day_without_birds?(list) do
    0 in list
  end

  def total([]) do
    0
  end
  def total([h | r]) do
    h + total(r)
  end

  def busy_days([]) do
    0
  end
  def busy_days([h | r]) do
    if h >= 5 do
      busy_days(r) + 1
    else
      busy_days(r)
    end
  end
end
