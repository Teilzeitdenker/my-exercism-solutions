# Use the Plot struct as it is provided
defmodule Plot do
  @enforce_keys [:plot_id, :registered_to]
  defstruct [:plot_id, :registered_to]
end

defmodule CommunityGarden do
  use Agent
  def start(_opts \\ []) do
    Agent.start(fn -> [next_number: 1, plot_list: []] end)
  end

  def list_registrations(pid) do
    Agent.get(pid, fn state -> state[:plot_list] end)
  end

  def register(pid, register_to) do
    id = Agent.get(pid, fn state -> state[:next_number] end)
    new_plot = %Plot{plot_id: id, registered_to: register_to}
    Agent.update(pid, fn state ->
      [next_number: state[:next_number] + 1,
      plot_list: [new_plot | state[:plot_list]]] end)
    new_plot
  end

  def release(pid, plot_id) do
    element_at =
      Agent.get(pid, fn state -> state[:plot_list] end)
      |> Enum.find_index(fn el -> el.plot_id == plot_id end)
    case element_at do
      nil -> :not_found
      n   -> Agent.update(pid, fn state ->
               [next_number: state[:next_number],
               plot_list: state[:plot_list] |> List.delete_at(n)] end)
             :ok
    end
  end

  def get_registration(pid, plot_id) do
    ind =
      Agent.get(pid, fn state -> state[:plot_list] end)
      |> Enum.find_index(fn el -> el.plot_id == plot_id end)
    case ind do
      nil -> {:not_found, "plot is unregistered"}
      n   -> Agent.get(pid, fn state ->
               state[:plot_list] |> Enum.at(n) end)
    end
  end
end
