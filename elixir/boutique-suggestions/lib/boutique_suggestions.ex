defmodule BoutiqueSuggestions do
  def get_combinations(tops, bottoms, options \\ []) do
    max_price = Keyword.get(options, :maximum_price, 100.00)
    for t <- tops,
        b <- bottoms,
        t.base_color != b.base_color,
        (t.price + b.price) <= max_price
        do
          {t, b}
        end
  end
end
