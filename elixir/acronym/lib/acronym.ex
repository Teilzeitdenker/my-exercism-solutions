defmodule Acronym do
  @doc """
  Generate an acronym from a string.
  "This is a string" => "TIAS"
  """
  @spec abbreviate(String.t()) :: String.t()
  def abbreviate(string) do
    delimiters = [" ", "-", "_"]
    string
    |> String.split(delimiters, trim: true)
    |> Enum.map(fn word -> word |> String.at(0) |> String.capitalize() end)
    |> Enum.join()
  end
end
