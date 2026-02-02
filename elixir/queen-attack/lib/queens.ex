defmodule Queens do
  @type t :: %Queens{black: {integer, integer}, white: {integer, integer}}
  defstruct [:white, :black]

  @doc """
  Creates a new set of Queens
  """
  @spec new(Keyword.t()) :: Queens.t()
  def new(opts \\ []) do
    case opts |> Keyword.keys() do
      [:white] -> case check_coords(Keyword.get(opts, :white)) do
        false -> raise ArgumentError
        true  -> %Queens{white: Keyword.get(opts, :white)}
      end
      [:black] -> case check_coords(Keyword.get(opts, :black)) do
        false -> raise ArgumentError
        true  -> %Queens{black: Keyword.get(opts, :black)}
      end
      [:white, :black] -> case {check_coords(Keyword.get(opts, :white)), check_coords(Keyword.get(opts, :black))} do
        {true, true} ->
          if Keyword.get(opts, :white) == Keyword.get(opts, :black) do
            raise ArgumentError
          else
            %Queens{white: Keyword.get(opts, :white), black: Keyword.get(opts, :black)}
          end
        _ -> raise ArgumentError
      end
      _ -> raise ArgumentError
    end
  end

  @spec check_coords({integer, integer}) :: boolean()
  defp check_coords({r, c}) do
    r in 0..7 and c in 0..7
  end

  @doc """
  Gives a string representation of the board with
  white and black queen locations shown
  """
  @spec to_string(Queens.t()) :: String.t()
  def to_string(queens) do
    0..7 |> Enum.map(&get_nth_row(&1, queens)) |> Enum.join("\n")
  end

  @spec get_nth_row(integer(), Queens.t()) :: String.t()
  defp get_nth_row(n, queens) do
    {wr, wc} = case queens.white do
      nil -> {-1, -1}
      {r, c} -> {r, c}
    end
    {br, bc} = case queens.black do
      nil -> {-1, -1}
      {r, c} -> {r, c}
    end
    case {wr, br} do
      {^n, ^n} -> 0..7 |> Enum.map(fn c ->
        case c do
          ^wc -> "W"
          ^bc -> "B"
          _  -> "_"
        end
      end) |> Enum.join(" ")
      {^n, _} -> 0..7 |> Enum.map(fn c ->
        case c do
          ^wc -> "W"
          _  -> "_"
        end
      end) |> Enum.join(" ")
      {_, ^n} -> 0..7 |> Enum.map(fn c ->
        case c do
          ^bc -> "B"
          _  -> "_"
        end
      end) |> Enum.join(" ")
      _  -> "_ _ _ _ _ _ _ _"
    end
  end

  @doc """
  Checks if the queens can attack each other
  """
  @spec can_attack?(Queens.t()) :: boolean
  def can_attack?(queens) do
    if is_nil(queens.black) or is_nil(queens.white) do
      false
    else
      {a, b} = queens.black
      {c, d} = queens.white
      a == c or b == d or abs(a - c) == abs(b - d)
    end
  end
end
