defmodule BoutiqueSuggestions do
  def get_combinations(tops, bottoms, options \\ [maximum_price: 100.00]) do
    for t <- tops,
        b <- bottoms,
        t.base_color != b.base_color,
        (t.price + b.price) <= options[:maximum_price]
        do
          {t, b}
        end
  end
end
