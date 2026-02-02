defmodule BankAccount do
  @moduledoc """
  A bank account that supports access from multiple processes.
  """

  @typedoc """
  An account handle.
  """
  @opaque account :: pid

  @doc """
  Open the bank. Makes the account available.
  """
  @spec open_bank() :: account
  def open_bank(), do: Agent.start(fn -> %{closed: false, balance: 0} end) |> elem(1)

  @doc """
  Close the bank. Makes the account unavailable.
  """
  @spec close_bank(account) :: :ok
  def close_bank(account), do: Agent.update(account, &Map.put(&1, :closed, true))

  @doc """
  Get the account's balance.
  """
  @spec balance(account) :: integer  | {:error, atom()}
  def balance(account) do
    Agent.get_and_update(account, fn state ->
      case state[:closed] do
        true -> {{:error, :account_closed}, state}
        false -> {state[:balance], state}
      end
    end)
  end

  @doc """
  Update the account's balance by adding the given amount which may be negative.
  """
  @spec update(account, integer) :: :ok | {:error, atom()}
  def update(account, amount) do
    Agent.get_and_update(account, fn state ->
      case state[:closed] do
        true -> {{:error, :account_closed}, state}
        false -> {:ok, %{state | balance: state[:balance] + amount}}
      end
    end)
  end
end
