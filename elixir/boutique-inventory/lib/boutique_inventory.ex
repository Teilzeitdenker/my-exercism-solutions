defmodule BoutiqueInventory do
  def sort_by_price(inventory) do
    # inventory |> Enum.sort(&((&1)[:price] <= (&2)[:price]))
    inventory |> Enum.sort_by(fn item -> item[:price] end)
  end

  def with_missing_price(inventory) do
    inventory |> Enum.filter(&is_nil((&1)[:price]))
  end

  def update_names(inventory, old_word, new_word) do
    # inventory |> Enum.map(&update_name(&1, old_word, new_word))
    inventory |> Enum.map(&Map.update!(&1, :name, fn val -> String.replace(val, old_word, new_word) end))
  end

  def increase_quantity(item, count) do
    Map.update!(item, :quantity_by_size, fn m -> Map.new(m, fn {k, v} -> {k, v + count} end) end)
  end

  def total_quantity(item) do
    Enum.reduce(item[:quantity_by_size], 0, fn {_, v}, acc -> acc + v end)
  end

  # brauch ich gar nicht...
  # defp update_name(item, old_word, new_word) do
  #   %{name: String.replace(item[:name], old_word, new_word), price: item[:price], quantity_by_size: item[:quantity_by_size]}
  # end
end
