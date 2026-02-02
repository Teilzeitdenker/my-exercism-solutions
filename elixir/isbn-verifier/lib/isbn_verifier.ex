defmodule IsbnVerifier do
  @doc """
    Checks if a string is a valid ISBN-10 identifier

    ## Examples

      iex> IsbnVerifier.isbn?("3-598-21507-X")
      true

      iex> IsbnVerifier.isbn?("3-598-2K507-0")
      false

  """
  @spec isbn?(String.t()) :: boolean
  def isbn?(isbn) do
    with {:ok, {digits_string, checksum_string} } <- split_input(isbn),
         {:ok, digits}                            <- parse_digits(digits_string),
         {:ok, checksum}                          <- parse_checksum(checksum_string)
    do
      (digits |> Enum.zip(10..1//-1) |> Enum.map(&Tuple.product/1) |> Enum.sum) + checksum |> Integer.mod(11) == 0
    end
  end

  defp split_input(input) do
    relevant_part = input |> String.replace("-", "")
    case relevant_part |> String.length() == 10 do
      false -> false
      true  -> {:ok, relevant_part |> String.split_at(9) }
    end
  end

  defp parse_digits(input) do
    parse_results = input |> String.codepoints() |> Enum.map(&Integer.parse/1)
    case parse_results |> Enum.all?(&is_tuple/1) do
      false -> false
      true -> {:ok, parse_results |> Enum.map(&elem(&1, 0))}
    end
  end

  defp parse_checksum(input) do
    case input |> String.replace("X", "10") |> Integer.parse() do
      :error -> false
      {checksum, _ } -> {:ok, checksum}
    end
  end
end
