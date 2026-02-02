defmodule Ledger do
  # formatting constants
  @date_width 10
  @description_width 25
  @amount_width 13
  @separator " | "
  @truncate_suffix "..."

  @doc """
  Format the given entries given a currency and locale
  """
  @type currency :: :usd | :eur
  @type locale :: :en_US | :nl_NL
  @type entry :: %{amount_in_cents: integer(), date: Date.t(), description: String.t()}

  @spec format_entries(currency(), locale(), list(entry())) :: String.t()
  def format_entries(currency, locale, entries) do
    # use case statement here instead of if - do - else - block
    header = case locale do
      :en_US -> "Date       | Description               | Change       "
      :nl_NL -> "Datum      | Omschrijving              | Verandering  "
    end
    # get rid of if - do - else by a better way of handling the newlines
    entry_lines =
      entries # stable sorting to the rescue
      |> Enum.sort_by(& &1[:date])
      |> Enum.sort_by(& &1[:description])
      |> Enum.sort_by(& &1[:amount_in_cents])
      |> Enum.map(&format_entry(currency, locale, &1))
    ([header | entry_lines] |> Enum.join("\n")) <> "\n"
  end

  # give every part its own format function
  defp format_date(date, locale) do
    {order, joiner} = case locale do
      :en_US -> {[date.month, date.day, date.year], "/"}
      :nl_NL -> {[date.day, date.month, date.year], "-"}
    end
    order
    |> Enum.map(&to_string/1)
    |> Enum.map(&String.pad_leading(&1, 2, "0"))
    |> Enum.join(joiner)
    |> String.pad_trailing(@date_width, " ")
  end

  defp format_description(descr) do
    if descr |> String.length > @description_width do
      to_idx = @description_width - String.length(@truncate_suffix) - 1
      String.slice(descr, 0..to_idx) <> @truncate_suffix
    else
      String.pad_trailing(descr, @description_width, " ")
    end
  end

  # use american english standard as a default
  defp format_cents_signless(cents, opts \\ [whole_sep: ",", decimal_sep: "."]) do
    # 1. work on the whole part formatting
    whole_part = div(abs(cents), 100)
    under_thousand_part = rem(whole_part, 1000)
    over_thousand_part = div(whole_part, 1000)
    whole_fmt = if over_thousand_part > 0 do
      to_string(over_thousand_part) <> opts[:whole_sep] <> to_string(under_thousand_part)
    else
      to_string(under_thousand_part)
    end
    # 2. format the decimal part
    decimal_part = rem(abs(cents), 100)
    decimal_fmt = decimal_part |> to_string() |> String.pad_leading(2, "0")
    # 3. put them together with the correct separator
    whole_fmt <> opts[:decimal_sep] <> decimal_fmt
  end

  defp format_amount(cents, locale, currency) do
    amount = case locale do
      :en_US -> format_cents_signless(cents)
      :nl_NL -> format_cents_signless(cents, whole_sep: ".", decimal_sep: ",")
    end
    # get the currency symbol
    symbol = case currency do
      :usd -> "$"
      :eur -> "€"
    end
    # use a simple case statement instead of a convoluted if - else - monstrum
    case {cents >= 0, locale} do
      {true , :en_US} -> " #{symbol}#{amount} " # no space between symbol and amount, but at the end
      {false, :en_US} -> "(#{symbol}#{amount})" # :accounting - standard with braces
      {true , :nl_NL} -> " #{symbol} #{amount} " # extra space between and at the end
      {false, :nl_NL} -> "#{symbol} -#{amount} " # only in this case put the sign back in
    end
    |> String.pad_leading(@amount_width, " ")
  end

  # employ the individual format functions to put it all together
  defp format_entry(currency, locale, entry) do
    [
      format_date(entry.date, locale),
      format_description(entry.description),
      format_amount(entry.amount_in_cents, locale, currency)
    ]
    |> Enum.join(@separator)
  end
end
