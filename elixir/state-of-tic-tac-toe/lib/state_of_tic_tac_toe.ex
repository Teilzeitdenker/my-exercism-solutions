defmodule StateOfTicTacToe do
  @doc """
  Determine the state a game of tic-tac-toe where X starts.
  """
  @spec game_state(board :: String.t()) :: {:ok, :win | :ongoing | :draw} | {:error, String.t()}
  def game_state(board) do
    with {:ok, game_over?} <- check_freqs(board)
    do
      check_wins(board, game_over?)
    end
  end

  defp check_freqs(board) do
    freqs = board
    |> String.split
    |> Enum.flat_map(&String.to_charlist/1)
    |> Enum.frequencies
    num_xs = if freqs[?X] == nil, do: 0, else: freqs[?X]
    num_os = if freqs[?O] == nil, do: 0, else: freqs[?O]
    game_over? = freqs[?.] == nil # return this additional info in the :ok case
    case {num_xs, num_os} do      # num_xs is either num_os or num_os + 1
      {a, b} when a == b or a == b + 1 -> {:ok, game_over?}
      {a, b} when a < b                -> {:error, "Wrong turn order: O started"}
      _                                -> {:error, "Wrong turn order: X went twice"}
    end
  end

  defp check_wins(board, game_over?) do
    x = 'XXX'
    o = 'OOO'
    x_wins = has_won?(x, board)
    o_wins = has_won?(o, board)
    case {x_wins, o_wins, game_over?} do
      {true, true, _   } -> {:error, "Impossible board: game should have ended after the game was won"}
      {true, _   , _   } -> {:ok, :win}
      { _  , true, _   } -> {:ok, :win}
      { _  , _   , true} -> {:ok, :draw}
      _                  -> {:ok, :ongoing}
    end
  end

  defp has_won?(player_triple, board) do
    all_poss(board) |> Enum.any?(fn el -> el == player_triple end)
  end

  defp all_poss(board) do
    get_rows(board)
    |> Enum.concat(get_cols(board))
    |> Enum.concat(get_diagonals(board))
  end

  defp get_rows(board) do
    board
    |> String.split
    |> Enum.map(&String.to_charlist/1)
  end

  defp get_cols(board) do
    board
    |> String.split
    |> Enum.map(&String.to_charlist/1)
    |> Enum.zip()
    |> Enum.map(&Tuple.to_list/1)
  end

  defp get_diagonals(board) do
    diag_ids = [0, 1, 2]
    rev_ids = [2, 1, 0]
    [
      get_rows(board)
      |> Enum.zip(diag_ids)
      |> Enum.map(fn {ls, i} -> ls |> Enum.at(i) end),
      get_rows(board)
      |> Enum.zip(rev_ids)
      |> Enum.map(fn {ls, i} -> ls |> Enum.at(i) end)
    ]
  end
end
