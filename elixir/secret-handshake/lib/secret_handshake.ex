defmodule SecretHandshake do
  use Bitwise, only_operators: true
  @doc """
  Determine the actions of a secret handshake based on the binary
  representation of the given `code`.

  If the following bits are set, include the corresponding action in your list
  of commands, in order from lowest to highest.

  1 = wink
  10 = double blink
  100 = close your eyes
  1000 = jump

  10000 = Reverse the order of the operations in the secret handshake
  """
  @spec commands(code :: integer) :: list(String.t())
  def commands(code) do
    [
      &(&1 ++ ["wink"]),
      &(&1 ++ ["double blink"]),
      &(&1 ++ ["close your eyes"]),
      &(&1 ++ ["jump"]),
      &Enum.reverse/1]
    |> Enum.with_index(fn function, index -> {1 <<< index , function} end)
    |> Enum.filter(fn {power, _} -> (code &&& power) == power end)
    |> Enum.reduce([], fn {_, function}, acc -> apply(function, [acc]) end)
  end
end
