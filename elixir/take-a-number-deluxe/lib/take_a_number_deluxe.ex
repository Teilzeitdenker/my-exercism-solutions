defmodule TakeANumberDeluxe do
  use GenServer
  # Client API

  @spec start_link(keyword()) :: {:ok, pid()} | {:error, atom()}
  def start_link(init_arg) do
    min_number = Keyword.get(init_arg, :min_number)
    max_number = Keyword.get(init_arg, :max_number)
    auto_shutdown_timeout = Keyword.get(init_arg, :auto_shutdown_timeout, :infinity)
    case TakeANumberDeluxe.State.new(min_number, max_number, auto_shutdown_timeout) do
      {:ok, state} ->
        GenServer.start_link(__MODULE__, state) # this yields {:ok, pid}
      {:error, error} ->
        {:error, error}
    end
  end

  @spec report_state(pid()) :: TakeANumberDeluxe.State.t()
  def report_state(machine) do
    GenServer.call(machine, :get_state)
  end

  @spec queue_new_number(pid()) :: {:ok, integer()} | {:error, atom()}
  def queue_new_number(machine) do
    case GenServer.call(machine, :enqueue) do
      num when is_integer(num)  -> {:ok, num}
      error when is_atom(error) -> {:error, error}
      other                     -> {:error, other}
    end
  end

  @spec serve_next_queued_number(pid(), integer() | nil) :: {:ok, integer()} | {:error, atom()}
  def serve_next_queued_number(machine, priority_number \\ nil) do
    case GenServer.call(machine, {:dequeue, priority_number}) do
      num when is_integer(num)  -> {:ok, num}
      error when is_atom(error) -> {:error, error}
      other                     -> {:error, other}
    end
  end

  @spec reset_state(pid()) :: :ok
  def reset_state(machine) do
    GenServer.cast(machine, :reset)
  end

  # Server callbacks
  @impl GenServer
  def init(state) do
    {:ok, state, state.auto_shutdown_timeout}
  end

  @impl GenServer
  def handle_call(:get_state, _from, state) do
    {:reply, state, state, state.auto_shutdown_timeout}
  end
  def handle_call(:enqueue, _from, state) do
    case TakeANumberDeluxe.State.queue_new_number(state) do
      {:ok, new_number, new_state} -> {:reply, new_number, new_state, new_state.auto_shutdown_timeout}
      {:error, error} -> {:reply, error, state, state.auto_shutdown_timeout}
    end
  end
  def handle_call({:dequeue, priority_number}, _from, state) do
    case TakeANumberDeluxe.State.serve_next_queued_number(state, priority_number) do
      {:ok, next_number, new_state} -> {:reply, next_number, new_state, new_state.auto_shutdown_timeout}
      {:error, error} -> {:reply, error, state, state.auto_shutdown_timeout}
    end
  end

  @impl GenServer
  def handle_cast(:reset, state) do
    {:ok, new_state} = TakeANumberDeluxe.State.new(state.min_number, state.max_number, state.auto_shutdown_timeout)
    {:noreply, new_state, new_state.auto_shutdown_timeout}
  end

  @impl GenServer
  def handle_info(:timeout, state) do
    {:stop, :normal, state}
  end
  def handle_info(_msg, state) do
    {:noreply, state, state.auto_shutdown_timeout}
  end
end
