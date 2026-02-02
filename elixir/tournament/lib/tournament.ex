defmodule Tournament do
  # use TypeCheck

  # NOTE: writing "use TypeCheck.ExUnit" and "spectest Tournament" into the test file yields automatic additional spectests
  # for every specification done with an exclamation mark @spec!
  # One has to add the following dependencies in mix.exs:
  #     {:type_check, "~> 0.13.3"},
  # To allow spectesting and property-testing data generators (optional):
  #    {:stream_data, "~> 0.5.0", only: :test},

  defmodule MatchResult do
    defstruct [:team, :result, :points]
    # @type! t :: %__MODULE__{team: String.t(), result: String.t(), points: non_neg_integer()}
  end

  defmodule TeamStats do
    defstruct [:name, :played, :win, :draw, :loss, :points]
    # @type! t :: %__MODULE__{name: String.t(), played: non_neg_integer(), win: non_neg_integer(), draw: non_neg_integer(), loss: non_neg_integer(), points: non_neg_integer()}
  end
  @doc """
  Given `input` lines representing two teams and whether the first of them won,
  lost, or reached a draw, separated by semicolons, calculate the statistics
  for each team's number of games played, won, drawn, lost, and total points
  for the season, and return a nicely-formatted string table.

  A win earns a team 3 points, a draw earns 1 point, and a loss earns nothing.

  Order the outcome by most total points for the season, and settle ties by
  listing the teams in alphabetical order.
  """
  # @spec! tally(input :: list(String.t())) :: String.t()
  def tally(input) do
    header = "Team                           | MP |  W |  D |  L |  P"
    formatted_team_rows =
      input
      |> Enum.map(&String.split(&1, ";"))
      |> Enum.flat_map(&row_to_match_result/1)
      |> Enum.group_by(&(&1.team))
      |> Enum.map(&group_to_teamstats/1)
      |> Enum.sort_by(&(&1.name))
      |> Enum.sort_by(&(&1.points), &>=/2)
      |> Enum.map(&teamstats_to_formatted_string/1)
    [header | formatted_team_rows] |> Enum.join("\n")
  end

  # @spec! row_to_match_result(list(String.t())) :: list(MatchResult.t())
  defp row_to_match_result(ls) do
    if ls |> Enum.count() != 3 do
      []
    else
      [team_a, team_b, result] = ls
      case result do
        "win"  -> [%MatchResult{team: team_a, result: result, points: 3}, %MatchResult{team: team_b, result: "loss", points: 0}]
        "draw" -> [%MatchResult{team: team_a, result: result, points: 1}, %MatchResult{team: team_b, result: result, points: 1}]
        "loss" -> [%MatchResult{team: team_a, result: result, points: 0}, %MatchResult{team: team_b, result: "win",  points: 3}]
        _      -> []
      end
    end
  end

  defp group_to_teamstats({team, results}) do
    %TeamStats{
      name: team,
      played: results |> Enum.count,
      win: results |> Enum.filter(&(&1.result == "win")) |> Enum.count,
      draw: results |> Enum.filter(&(&1.result == "draw")) |> Enum.count,
      loss: results |> Enum.filter(&(&1.result == "loss")) |> Enum.count,
      points: results |> Enum.map(&(&1.points)) |> Enum.sum }
  end

  # @spec! teamstats_to_formatted_string(TeamStats.t()) :: String.t()
  defp teamstats_to_formatted_string(stats) do
    fmt_name = stats.name |> String.pad_trailing(30)
    fmt_played = stats.played |> to_string() |> String.pad_leading(2)
    fmt_win = stats.win |> to_string() |> String.pad_leading(2)
    fmt_draw = stats.draw |> to_string() |> String.pad_leading(2)
    fmt_loss = stats.loss |> to_string() |> String.pad_leading(2)
    fmt_points = stats.points |> to_string() |> String.pad_leading(2)
    "#{fmt_name} | #{fmt_played} | #{fmt_win} | #{fmt_draw} | #{fmt_loss} | #{fmt_points}"
  end
end
