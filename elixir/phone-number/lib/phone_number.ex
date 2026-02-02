defmodule PhoneNumber do

  @no_digit ~r/[^[:digit:]]/
  @punctuation ~r/[\s-\.\(\)\+]/

  @doc """
  Remove formatting from a phone number if the given number is valid. Return an error otherwise.
  """
  @spec clean(String.t()) :: {:ok, String.t()} | {:error, String.t()}
  def clean(input) do
    with {:ok, only_digits} <- validate_input(input),
         {:ok, ten_digits} <- check_length(only_digits),
         {:ok, good_area_code} <- check_area_code(ten_digits),
         {:ok, good_exchange_code} <- check_exchange_code(good_area_code)
    do
      {:ok, good_exchange_code}
    end
  end

  defp validate_input(input) do
    only_digits = Regex.replace(@punctuation, input, "")
    if Regex.match?(@no_digit, only_digits) do
      {:error, "must contain digits only"}
    else
      {:ok, only_digits}
    end
  end

  defp check_length(input) do
    case input |> String.length() do
      a when a > 11 or a < 10 -> {:error, "incorrect number of digits"}
      11 -> if input |> String.starts_with?("1"), do: {:ok, input |> String.slice(1..-1//1)}, else: {:error, "11 digits must start with 1"}
      10 -> {:ok, input}
    end
  end

  defp check_area_code(input) do
    case input |> String.at(0) do
      "0" -> {:error, "area code cannot start with zero"}
      "1" -> {:error, "area code cannot start with one"}
      _   -> {:ok, input}
    end
  end

  defp check_exchange_code(input) do
    case input |> String.at(3) do
      "0" -> {:error, "exchange code cannot start with zero"}
      "1" -> {:error, "exchange code cannot start with one"}
      _   -> {:ok, input}
    end
  end

end
