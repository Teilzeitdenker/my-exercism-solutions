defmodule CircularBuffer do
  @moduledoc """
  An API to a stateful process that fills and empties a circular buffer
  """

  @doc """
  Create a new buffer of a given capacity
  """
  @spec new(capacity :: integer) :: {:ok, pid}
  def new(capacity) do
    Agent.start_link(fn -> %{items: [], capacity: capacity} end)
  end

  @doc """
  Read the oldest entry in the buffer, fail if it is empty
  """
  @spec read(buffer :: pid) :: {:ok, any} | {:error, atom}
  def read(buffer) do
    case Agent.get(buffer, &Map.get(&1, :items)) do
      [] ->
        {:error, :empty}
      [el | rest] ->
        Agent.update(buffer, &Map.put(&1, :items, rest))
        {:ok, el}
    end
  end

  @doc """
  Write a new item in the buffer, fail if is full
  """
  @spec write(buffer :: pid, item :: any) :: :ok | {:error, atom}
  def write(buffer, item) do
    capacity = Agent.get(buffer, &Map.get(&1, :capacity))
    items = Agent.get(buffer, &Map.get(&1, :items))
    case items |> Enum.count() do
      ^capacity ->
        {:error, :full}
      _ ->
        Agent.update(buffer, &Map.put(&1, :items, items ++ [item]))
        :ok
    end
  end

  @doc """
  Write an item in the buffer, overwrite the oldest entry if it is full
  """
  @spec overwrite(buffer :: pid, item :: any) :: :ok
  def overwrite(buffer, item) do
    capacity = Agent.get(buffer, &Map.get(&1, :capacity))
    items = Agent.get(buffer, &Map.get(&1, :items))
    case items |> Enum.count() do
      ^capacity ->
        [_h | t] = items
        Agent.update(buffer, &Map.put(&1, :items, t ++ [item]))
        :ok
      _ ->
        Agent.update(buffer, &Map.put(&1, :items, items ++ [item]))
        :ok
    end
  end

  @doc """
  Clear the buffer
  """
  @spec clear(buffer :: pid) :: :ok
  def clear(buffer) do
    Agent.update(buffer, &Map.put(&1, :items, []))
    :ok
  end
end
