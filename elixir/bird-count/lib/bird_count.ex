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

  def total(list) do
    List.foldr(list, 0, fn x, acc -> acc + x end)
  end

  def busy_days(list) do
    List.foldr(list, 0, fn
      x, acc when x >= 5 -> acc + 1
      _, acc -> acc
     end)
  end
end
