defmodule TakeANumber do
  def start() do
    spawn(fn ->
      loop()
    end)
  end

  def loop(state \\ 0) do
    receive do
      {:report_state, sender} ->
        send(sender, state)
        loop(state)
      {:take_a_number, sender} ->
        state = state + 1
        send(sender, state)
        loop(state)
      :stop -> :stopped
      _ -> loop(state)
    end
  end
end
