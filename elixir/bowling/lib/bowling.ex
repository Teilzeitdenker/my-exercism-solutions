defmodule Bowling do
  defstruct [score: 0, frames: 0, pins_left: 10, roll_count: 0, bonus1: false, bonus2: false]
  @doc """
    Creates a new game of bowling that can be used to store the results of
    the game
  """
  @spec start() :: %Bowling{}
  def start, do: %Bowling{}

  @doc """
    Records the number of pins knocked down on a single roll. Returns `%Bowling{}`
    unless there is something wrong with the given number of pins, in which
    case it returns a helpful error tuple.
  """
  @spec roll(%Bowling{}, integer) :: {:ok, %Bowling{}} | {:error, String.t()}
  def roll(game, roll) do
    cond do
      complete?(game)       -> {:error, "Cannot roll after game is over"}
      roll < 0              -> {:error, "Negative roll is invalid"}
      roll > game.pins_left -> {:error, "Pin count exceeds pins on the lane"}
      true                  -> {:ok, do_roll(game, roll)}
    end
  end

  @doc """
    Returns the score of a given game of bowling if the game is complete.
    If the game isn't complete, it returns a helpful error tuple.
  """
  @spec score(%Bowling{}) :: {:ok, integer} | {:error, String.t()}
  def score(game) do
    if complete?(game), do: {:ok, game.score}, else: {:error, "Score cannot be taken until the end of the game"}
  end

  @spec do_roll(%Bowling{}, integer) :: %Bowling{}
  defp do_roll(game, roll) do
    factor = ((last_frame?(game) && 0 || 1) + (game.bonus1 && 1 || 0) + (game.bonus2 && 1 || 0))
    game = %{game | pins_left: game.pins_left - roll, score: game.score + roll * factor, bonus1: game.bonus2}
    game = %{game | bonus2: false, roll_count: game.roll_count + 1}
    if game.roll_count == 1 && game.pins_left > 0 do
      game
    else
      if game.pins_left == 0 && !last_frame?(game) do
        repl_key = if game.roll_count == 1, do: :bonus2, else: :bonus1
        game = Map.replace!(game, repl_key, true)
        start_new_frame(game)
      else
        start_new_frame(game)
      end
    end
  end

  defp complete?(game), do: last_frame?(game) && !game.bonus1 && !game.bonus2
  defp last_frame?(game), do: game.frames >= 10
  defp start_new_frame(game), do: %{ game | roll_count: 0, pins_left: 10, frames: game.frames + 1}
end
