defmodule LinkedList do
  @opaque t :: tuple()

  @doc """
  Construct a new LinkedList
  """
  @spec new() :: t
  def new() do
    {true, []}
  end

  @doc """
  Push an item onto a LinkedList
  """
  @spec push(t, any()) :: t
  def push(list, elem) do
    {false, [elem | elem(list, 1)]}
  end

  @doc """
  Counts the number of elements in a LinkedList
  """
  @spec count(t) :: non_neg_integer()
  def count(list) do
    Enum.count(elem(list, 1))
  end

  @doc """
  Determine if a LinkedList is empty
  """
  @spec empty?(t) :: boolean()
  def empty?(list) do
    elem(list, 0)
  end

  @doc """
  Get the value of a head of the LinkedList
  """
  @spec peek(t) :: {:ok, any()} | {:error, :empty_list}
  def peek(list) do
    if empty?(list), do: {:error, :empty_list}, else: {:ok, hd(elem(list, 1))}
  end

  @doc """
  Get tail of a LinkedList
  """
  @spec tail(t) :: {:ok, t} | {:error, :empty_list}
  def tail(list) do
    case pop(list) do
      {:error, :empty_list} -> {:error, :empty_list}
      {:ok, _, rest}        -> {:ok, rest}
    end
  end

  @doc """
  Remove the head from a LinkedList
  """
  @spec pop(t) :: {:ok, any(), t} | {:error, :empty_list}
  def pop(list) do
    if empty?(list) do
      {:error, :empty_list}
    else
      [ fst | rest] = elem(list, 1)
      if rest == [], do: {:ok, fst, new() }, else: {:ok, fst, {false, rest} }
    end
  end

  @doc """
  Construct a LinkedList from a stdlib List
  """
  @spec from_list(list()) :: t
  def from_list(list) do
    if Enum.empty?(list), do: new(), else: {false, list}
  end

  @doc """
  Construct a stdlib List LinkedList from a LinkedList
  """
  @spec to_list(t) :: list()
  def to_list(list) do
    elem(list, 1)
  end

  @doc """
  Reverse a LinkedList
  """
  @spec reverse(t) :: t
  def reverse(list) do
    {elem(list, 0), elem(list, 1) |> Enum.reverse}
  end
end
