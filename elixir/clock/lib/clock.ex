# import Kernel, except: [to_string: 1]

defmodule Clock do
  defstruct minute: 0

  @type t :: %__MODULE__{}
  @day 1440

  defimpl String.Chars do
    defp pad_with_zeros(n) do
      Kernel.to_string(n) |> String.pad_leading(2, "0")
    end
    def to_string(%Clock{minute: minute}) do
      "#{pad_with_zeros(div(minute, 60))}:#{pad_with_zeros(rem(minute, 60))}"
    end
  end

  @doc """
  Returns a clock that can be represented as a string:

      iex> Clock.new(8, 9) |> to_string
      "08:09"
  """
  @spec new(integer, integer) :: t()
  def new(hour, minute) do
    minutes = hour * 60 + minute
    %Clock{minute: Integer.mod(minutes, @day)}
  end

  @doc """
  Adds two clock times:

      iex> Clock.new(10, 0) |> Clock.add(3) |> to_string
      "10:03"
  """
  @spec add(t(), integer) :: t()
  def add(%Clock{minute: minute}, add_minute) do
    %Clock{minute: Integer.mod(minute + add_minute, @day)}
  end
  # use Integer.mod() instead, this is the real modulo function and not the remainder
  # defp modulo(a, b) do
  #   rem(rem(a, b) + b, b)
  # end
end
