import Kernel, except: [to_string: 1]

defmodule Clock do
  defstruct hour: 0, minute: 0

  @type t :: %__MODULE__{}
  @day 1440

  defimpl String.Chars do
    def to_string(%Clock{hour: _hour, minute: minute}) do
      Time.new!(div(minute, 60), rem(minute, 60), 0) |> Kernel.to_string() |> to_charlist() |> Enum.take(5) |> Kernel.to_string()
    end
  end

  @doc """
  Returns a clock that can be represented as a string:

      iex> Clock.new(8, 9) |> to_string
      "08:09"
  """
  @spec new(integer, integer) :: t()
  def new(hour, minute) do
    raw = hour * 60 + minute
    %Clock{hour: 0, minute: modulo(raw, @day)}
  end

  @doc """
  Adds two clock times:

      iex> Clock.new(10, 0) |> Clock.add(3) |> to_string
      "10:03"
  """
  @spec add(t(), integer) :: t()
  def add(%Clock{hour: hour, minute: minute}, add_minute) do
    raw = hour * 60 + minute
    %Clock{hour: 0, minute: modulo(raw + add_minute, @day)}
  end

  defp modulo(a, b) do
    rem(rem(a, b) + b, b)
  end
end
