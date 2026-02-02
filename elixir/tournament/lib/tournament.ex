defmodule Tournament do
  # NOTE: One has to "require Tournament" in iex> to make use of the macros (i.e. Tournament.match_result/0, 1 or 2) defined by "defrecord" there
  require Record
  Record.defrecord(:match_result, team: "Name", result: "loss", points: 0)
  @type match_result :: record(:match_result, team: String.t(), result: String.t(), points: non_neg_integer())

  defmodule TeamStats do
    defstruct [:name, :played, :win, :draw, :loss, :points]
    @type t :: %__MODULE__{name: String.t(), played: pos_integer(), win: non_neg_integer(), draw: non_neg_integer(), loss: non_neg_integer(), points: non_neg_integer()}
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
  @spec tally(input :: list(String.t())) :: String.t()
  def tally(input) do
    header = "Team                           | MP |  W |  D |  L |  P"
    teams =
      input
      |> Enum.map(&String.split(&1, ";"))
      |> Enum.filter(&(Enum.count(&1) == 3))
      |> Enum.flat_map(fn [team_a, team_b, result] ->
        case result do
          "win"  -> [match_result(team: team_a, result: result, points: 3), match_result(team: team_b, result: "loss", points: 0)]
          "draw" -> [match_result(team: team_a, result: result, points: 1), match_result(team: team_b, result: result, points: 1)]
          "loss" -> [match_result(team: team_a, result: result, points: 0), match_result(team: team_b, result: "win",  points: 3)]
          _      -> []
        end
      end)
      |> Enum.group_by(fn rec -> match_result(rec, :team) end)
      |> Enum.map(fn {team, results} -> %TeamStats{
        name: team,
        played: results |> Enum.count,
        win: results |> Enum.filter(&(match_result(&1, :result) == "win")) |> Enum.count,
        draw: results |> Enum.filter(&(match_result(&1, :result) == "draw")) |> Enum.count,
        loss: results |> Enum.filter(&(match_result(&1, :result) == "loss")) |> Enum.count,
        points: results |> Enum.map(&match_result(&1, :points)) |> Enum.sum }
      end)
      |> Enum.sort_by(fn stats -> stats.name end)
      |> Enum.sort_by(fn stats -> stats.points end, &>=/2)
      |> Enum.map(fn stats ->
        fmt_name = stats.name |> String.pad_trailing(30)
        fmt_played = stats.played |> to_string() |> String.pad_leading(2)
        fmt_win = stats.win |> to_string() |> String.pad_leading(2)
        fmt_draw = stats.draw |> to_string() |> String.pad_leading(2)
        fmt_loss = stats.loss |> to_string() |> String.pad_leading(2)
        fmt_points = stats.points |> to_string() |> String.pad_leading(2)
        "#{fmt_name} | #{fmt_played} | #{fmt_win} | #{fmt_draw} | #{fmt_loss} | #{fmt_points}"
      end)
    [header | teams] |> Enum.join("\n")
  end
end
