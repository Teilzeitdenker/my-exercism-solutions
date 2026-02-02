defmodule RPNCalculator.Exception do
  # Please implement DivisionByZeroError here.
  defmodule DivisionByZeroError do
    defexception message: "division by zero occurred"
  end
  # Please implement StackUnderflowError here.
  defmodule StackUnderflowError do
    defexception message: "stack underflow occurred"

    @impl true
    def exception(value) do
      case value do
        [] ->
          %StackUnderflowError{}
        _ ->
          %StackUnderflowError{message: "stack underflow occurred, context: " <> value}
      end
    end
  end
  def divide(numbers) do
    cond do
      length(numbers) < 2 -> raise StackUnderflowError, "when dividing"
      hd(numbers) == 0  -> raise DivisionByZeroError
      true -> div(hd(tl(numbers)), hd(numbers))
    end
  end
end
