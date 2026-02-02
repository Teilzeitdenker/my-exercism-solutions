defmodule React do
  @opaque cells :: pid
  @type cell :: {:input, String.t(), any} | {:output, String.t(), [String.t()], fun()}

  @doc """
  Start a reactive system
  """
  @spec new(cells :: [cell]) :: {:ok, pid}
  def new(cells) do
    Agent.start_link(fn ->
      Enum.reduce(cells, %{input_cells: %{}, compute_cells: %{}, cbs: %{}}, fn
        {:input,  str, val}         , acc -> put_in(acc.input_cells[str], val)
        {:output, str, ins, compute}, acc -> put_in(acc.compute_cells[str], {ins, compute})
      end)
    end)
  end

  @doc """
  Return the value of an input or output cell
  """
  @spec get_value(cells :: pid, c_name :: String.t()) :: any()
  def get_value(cells, c_name), do: Agent.get(cells, &get(&1, c_name))

  defp get(%{input_cells: ins}, c_name) when is_map_key(ins, c_name), do: ins[c_name]
  defp get(%{compute_cells: coms} = cells, c_name) when is_map_key(coms, c_name) do
    apply(elem(coms[c_name], 1), Enum.map(elem(coms[c_name], 0), &get(cells, &1)))
  end

  @doc """
  Set the value of an input cell
  """
  @spec set_value(cells :: pid, c_name :: String.t(), value :: any) :: :ok
  def set_value(cells, c_name, value) do
    Agent.update(cells, fn cells ->
      next_cells = put_in(cells.input_cells[c_name], value)
      for {cb_name, {cell, cb}} <- next_cells.cbs do
        new_value = get(next_cells, cell)
        if get(cells, cell) != new_value, do: cb.(cb_name, new_value)
      end
      next_cells
    end)
  end

  @doc """
  Add a callback to an output cell
  """
  @spec add_callback(cells :: pid, c_name :: String.t(), cb_name :: String.t(), cb :: fun()) :: :ok
  def add_callback(cells, c_name, cb_name, cb) do
    Agent.update(cells, &put_in(&1.cbs[cb_name], {c_name, cb}))
  end

  @doc """
  Remove a callback from an output cell
  """
  @spec remove_callback(cells :: pid, c_name :: String.t(), cb_name :: String.t()) :: :ok
  def remove_callback(cells, _c_name, cb_name) do
    Agent.update(cells, &elem(pop_in(&1.cbs[cb_name]), 1))
  end
end
