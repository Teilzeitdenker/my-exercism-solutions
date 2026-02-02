defmodule Forth do
  @opaque evaluator :: {any, any}

  @doc """
  Create a new evaluator.
  """
  @spec new() :: evaluator
  def new() do
    {[], %{
      "+" => &(Enum.slice(need(&1, 2), 0..-3//1) ++ [Enum.at(&1, -2) + Enum.at(&1, -1)]),
      "-" => &(Enum.slice(need(&1, 2), 0..-3//1) ++ [Enum.at(&1, -2) - Enum.at(&1, -1)]),
      "*" => &(Enum.slice(need(&1, 2), 0..-3//1) ++ [Enum.at(&1, -2) * Enum.at(&1, -1)]),
      "/" => &(Enum.slice(need(&1, 2), 0..-3//1) ++ [div(Enum.at(&1, -2), not_zero(Enum.at(&1, -1)))]),
      "dup" => &(&1 ++ [Enum.at(need(&1, 1), -1)]),
      "drop" => &Enum.slice(need(&1, 1), 0..-2//1),
      "swap" => &(Enum.slice(need(&1, 2), 0..-3//1) ++ [Enum.at(&1, -1), Enum.at(&1, -2)]),
      "over" => &(&1 ++ [Enum.at(need(&1, 2), -2)])
    }}
  end

  @doc """
  Evaluate an input string, updating the evaluator state.
  """
  @spec eval(evaluator, String.t()) :: evaluator
  def eval({stack, commands}, s) do
    case (
      s
      |> String.downcase
      |> String.replace(" ", " ")
      |> String.split(~r/[\x00- ]|\s/u)
    ) do
      [":", name | definition] ->
        if name =~ ~r/^-?\d+$/ do
          raise(Forth.InvalidWord, word: name)
        else
          {func, [";" | rest]} = Enum.split_while(definition, &(&1 != ";"))
          str = func |> Enum.join(" ")
          {stack, Map.put(commands, name, &elem(eval({&1, commands}, str), 0))}
          |> eval(rest |> Enum.join(" "))
        end
      ops ->
        Enum.reduce(ops, stack, fn op, stack ->
          cond do
            Map.has_key?(commands, op) -> commands[op].(stack)
            op =~ ~r/^-?\d+$/ -> stack ++ [String.to_integer(op)]
            op == "" -> stack
            true -> raise(Forth.UnknownWord, word: op)
          end
        end)
        |> then(&{&1, commands})
    end
  end

  @doc """
  Return the current stack as a string with the element on top of the stack
  being the rightmost element in the string.
  """
  @spec format_stack(evaluator) :: String.t()
  def format_stack({stack, _}) do
    stack |> Enum.join(" ")
  end

  defmodule StackUnderflow do
    defexception []
    def message(_), do: "stack underflow"
  end

  defmodule InvalidWord do
    defexception word: nil
    def message(e), do: "invalid word: #{inspect(e.word)}"
  end

  defmodule UnknownWord do
    defexception word: nil
    def message(e), do: "unknown word: #{inspect(e.word)}"
  end

  defmodule DivisionByZero do
    defexception []
    def message(_), do: "division by zero"
  end

  defp need(l, n) when length(l) < n, do: raise(StackUnderflow)
  defp need(l, _), do: l

  defp not_zero(0), do: raise(DivisionByZero)
  defp not_zero(n), do: n
end
