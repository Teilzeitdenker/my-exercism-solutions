defmodule OcrNumbers do

  @ocr_digits [
    " _     _  _     _  _  _  _  _ ",
    "| |  | _| _||_||_ |_   ||_||_|",
    "|_|  ||_  _|  | _||_|  ||_| _|",
    "                              "
  ]

  @doc """
  Given a 3 x 4 grid of pipes, underscores, and spaces, determine which number is represented, or
  whether it is garbled.
  """
  @spec convert([String.t()]) :: {:ok, String.t()} | {:error, String.t()}
  def convert(input) do
    cond do
      rem(input |> Enum.count(), 4) != 0                 -> {:error, "invalid line count"}
      rem(input |> Enum.at(0) |> String.length , 3) != 0 -> {:error, "invalid column count"}
      true ->
        digit_to_char_map =
          @ocr_digits
          |> Enum.map(&chunks_of_3/1)
          |> transpose |> Enum.with_index
          |> Enum.map(fn { str, ind } -> {str, ind |> to_string()}  end )
          |> Map.new
        {:ok,
          input
          |> Enum.chunk_every(4)
          |> Enum.map(fn row ->
            row
            |> Enum.map(&chunks_of_3/1)
            |> transpose
            |> Enum.map(&decode(&1, digit_to_char_map))
            |> Enum.join("") end)
          |> Enum.join(",")
        }
    end
  end

  @spec chunks_of_3(String.t()) :: [String.t()]
  def chunks_of_3(s) do
    s |> String.codepoints() |> Enum.chunk_every(3) |> Enum.map(&Enum.join/1)
  end

  @spec transpose([[String.t()]]) :: [[String.t()]]
  def transpose([]), do: []
  def transpose([[]|_]), do: []
  def transpose(list) do
    [ Enum.map(list, &hd/1) | transpose(Enum.map(list, &tl/1)) ]
  end

  @spec decode(String.t(), %{}) :: String.t()
  def decode(s, m) do
    if m |> Map.has_key?(s) do
      m |> Map.fetch!(s)
    else
      "?"
    end
  end
end
