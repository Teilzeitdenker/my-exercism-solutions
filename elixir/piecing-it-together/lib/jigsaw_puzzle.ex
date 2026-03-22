defmodule JigsawPuzzle do
  @doc """
  Fill in missing jigsaw puzzle details from partial data
  """

  @type format() :: :landscape | :portrait | :square
  @type t() :: %__MODULE__{
          pieces: pos_integer() | nil,
          rows: pos_integer() | nil,
          columns: pos_integer() | nil,
          format: format() | nil,
          aspect_ratio: float() | nil,
          border: pos_integer() | nil,
          inside: pos_integer() | nil
        }

  defstruct [:pieces, :rows, :columns, :format, :aspect_ratio, :border, :inside]

  @spec data(jigsaw_puzzle :: JigsawPuzzle.t()) ::
          {:ok, JigsawPuzzle.t()} | {:error, String.t()}
  def data(puzzle) do
    with {:ok, p} <- derive_rows_columns(puzzle),
         {:ok, p} <- fill_all_fields(p),
         :ok <- check_consistency(puzzle, p) do
      {:ok, p}
    end
  end

  # ---------------------------------------------------------------------------
  # Step 1: Determine rows and columns from whatever fields are given
  # ---------------------------------------------------------------------------

  defp derive_rows_columns(p) do
    cond do
      # Both already known
      not is_nil(p.rows) and not is_nil(p.columns) ->
        {:ok, p}

      # rows + aspect_ratio  →  columns = rows * ar
      not is_nil(p.rows) and not is_nil(p.aspect_ratio) ->
        {:ok, %{p | columns: round(p.rows * p.aspect_ratio)}}

      # columns + aspect_ratio  →  rows = columns / ar
      not is_nil(p.columns) and not is_nil(p.aspect_ratio) ->
        {:ok, %{p | rows: round(p.columns / p.aspect_ratio)}}

      # rows + square format
      not is_nil(p.rows) and p.format == :square ->
        {:ok, %{p | columns: p.rows}}

      # columns + square format
      not is_nil(p.columns) and p.format == :square ->
        {:ok, %{p | rows: p.columns}}

      # pieces + rows  →  columns = pieces / rows
      not is_nil(p.pieces) and not is_nil(p.rows) ->
        cols = div(p.pieces, p.rows)
        if cols * p.rows == p.pieces,
          do: {:ok, %{p | columns: cols}},
          else: {:error, "Insufficient data"}

      # pieces + columns  →  rows = pieces / columns
      not is_nil(p.pieces) and not is_nil(p.columns) ->
        rows = div(p.pieces, p.columns)
        if rows * p.columns == p.pieces,
          do: {:ok, %{p | rows: rows}},
          else: {:error, "Insufficient data"}

      # rows + border  →  columns = (border - 2*rows + 4) / 2
      not is_nil(p.rows) and not is_nil(p.border) ->
        num = p.border - 2 * p.rows + 4
        if rem(num, 2) == 0 and div(num, 2) > 0,
          do: {:ok, %{p | columns: div(num, 2)}},
          else: {:error, "Insufficient data"}

      # columns + border  →  rows = (border - 2*columns + 4) / 2
      not is_nil(p.columns) and not is_nil(p.border) ->
        num = p.border - 2 * p.columns + 4
        if rem(num, 2) == 0 and div(num, 2) > 0,
          do: {:ok, %{p | rows: div(num, 2)}},
          else: {:error, "Insufficient data"}

      # rows + inside  →  columns = inside / (rows-2) + 2
      not is_nil(p.rows) and not is_nil(p.inside) ->
        if p.rows > 2 and rem(p.inside, p.rows - 2) == 0,
          do: {:ok, %{p | columns: div(p.inside, p.rows - 2) + 2}},
          else: {:error, "Insufficient data"}

      # columns + inside  →  rows = inside / (columns-2) + 2
      not is_nil(p.columns) and not is_nil(p.inside) ->
        if p.columns > 2 and rem(p.inside, p.columns - 2) == 0,
          do: {:ok, %{p | rows: div(p.inside, p.columns - 2) + 2}},
          else: {:error, "Insufficient data"}

      # pieces + aspect_ratio  →  rows = sqrt(pieces / ar)
      not is_nil(p.pieces) and not is_nil(p.aspect_ratio) ->
        case find_rows_cols_by_ratio(p.pieces, p.aspect_ratio) do
          {:ok, {rows, cols}} -> {:ok, %{p | rows: rows, columns: cols}}
          error -> error
        end

      # pieces + square
      not is_nil(p.pieces) and p.format == :square ->
        s = round(:math.sqrt(p.pieces))
        if s * s == p.pieces,
          do: {:ok, %{p | rows: s, columns: s}},
          else: {:error, "Insufficient data"}

      # inside + aspect_ratio  →  solve ar*r^2 - 2*(1+ar)*r + (4-inside) = 0
      not is_nil(p.inside) and not is_nil(p.aspect_ratio) ->
        case find_rows_cols_from_inside_ratio(p.inside, p.aspect_ratio) do
          {:ok, {rows, cols}} -> {:ok, %{p | rows: rows, columns: cols}}
          error -> error
        end

      # inside + square  →  rows = sqrt(inside) + 2
      not is_nil(p.inside) and p.format == :square ->
        inner = round(:math.sqrt(p.inside))
        if inner * inner == p.inside,
          do: {:ok, %{p | rows: inner + 2, columns: inner + 2}},
          else: {:error, "Insufficient data"}

      # border + pieces  →  solve quadratic: r + c = (border+4)/2, r*c = pieces
      not is_nil(p.border) and not is_nil(p.pieces) ->
        find_rows_cols_from_border_pieces(p, p.pieces)

      # border + inside  →  derive pieces first
      not is_nil(p.border) and not is_nil(p.inside) ->
        find_rows_cols_from_border_pieces(p, p.border + p.inside)

      true ->
        {:error, "Insufficient data"}
    end
  end

  # pieces / ar = rows^2  →  rows = sqrt(pieces / ar)
  # Fallback: scan divisors for exact integer match.
  defp find_rows_cols_by_ratio(n, ar) do
    rows = round(:math.sqrt(n / ar))
    cols = round(rows * ar)

    if rows > 0 and cols > 0 and rows * cols == n do
      {:ok, {rows, cols}}
    else
      result =
        Enum.find_value(1..floor(:math.sqrt(n)), fn i ->
          if rem(n, i) == 0 do
            j = div(n, i)

            cond do
              float_eq?(j / i, ar) -> {:ok, {i, j}}
              float_eq?(i / j, ar) -> {:ok, {j, i}}
              true -> nil
            end
          end
        end)

      result || {:error, "Insufficient data"}
    end
  end

  # inside = (rows-2)*(cols-2),  cols = ar*rows
  # → ar*rows^2 - 2*(1+ar)*rows + (4-inside) = 0
  defp find_rows_cols_from_inside_ratio(inside, ar) do
    a = ar
    b = -2.0 * (1.0 + ar)
    c = 4.0 - inside
    disc = b * b - 4.0 * a * c

    if disc < 0.0 do
      {:error, "Insufficient data"}
    else
      rows = round((-b + :math.sqrt(disc)) / (2.0 * a))
      cols = round(rows * ar)

      if rows > 0 and cols > 0,
        do: {:ok, {rows, cols}},
        else: {:error, "Insufficient data"}
    end
  end

  # rows + cols = (border+4)/2 = s
  # rows * cols = pieces
  # → t^2 - s*t + pieces = 0
  defp find_rows_cols_from_border_pieces(p, pieces) do
    s = div(p.border + 4, 2)
    disc = s * s - 4 * pieces

    if disc < 0 do
      {:error, "Insufficient data"}
    else
      sqrt_d = round(:math.sqrt(disc))

      if sqrt_d * sqrt_d != disc do
        {:error, "Insufficient data"}
      else
        r1 = div(s + sqrt_d, 2)
        r2 = div(s - sqrt_d, 2)
        {rows, cols} = orient_by_format(p, r1, r2)

        if rows > 0 and cols > 0,
          do: {:ok, %{p | rows: rows, columns: cols}},
          else: {:error, "Insufficient data"}
      end
    end
  end

  # r1 >= r2.  Portrait → rows > cols;  landscape → cols > rows.
  defp orient_by_format(p, r1, r2) do
    cond do
      p.format == :portrait -> {r1, r2}
      p.format == :landscape -> {r2, r1}
      not is_nil(p.aspect_ratio) and float_eq?(r2 / r1, p.aspect_ratio) -> {r1, r2}
      not is_nil(p.aspect_ratio) -> {r2, r1}
      true -> {r1, r2}
    end
  end

  # ---------------------------------------------------------------------------
  # Step 2: Compute all derived fields from rows + columns
  # ---------------------------------------------------------------------------

  defp fill_all_fields(p) do
    pieces = p.rows * p.columns
    border = 2 * p.rows + 2 * p.columns - 4
    inside = pieces - border
    aspect_ratio = p.columns / p.rows
    format = format_from_ratio(aspect_ratio)
    {:ok, %{p | pieces: pieces, border: border, inside: inside, aspect_ratio: aspect_ratio, format: format}}
  end

  defp format_from_ratio(ar) do
    cond do
      ar < 1.0 -> :portrait
      ar > 1.0 -> :landscape
      true -> :square
    end
  end

  # ---------------------------------------------------------------------------
  # Step 3: Verify original non-nil fields are consistent with derived values
  # ---------------------------------------------------------------------------

  defp check_consistency(orig, derived) do
    integer_fields_ok =
      [:pieces, :rows, :columns, :border, :inside, :format]
      |> Enum.all?(fn field ->
        val = Map.get(orig, field)
        is_nil(val) or val == Map.get(derived, field)
      end)

    ar_ok =
      is_nil(orig.aspect_ratio) or
        float_eq?(orig.aspect_ratio, derived.aspect_ratio)

    if integer_fields_ok and ar_ok, do: :ok, else: {:error, "Contradictory data"}
  end

  defp float_eq?(a, b, eps \\ 1.0e-9), do: abs(a - b) < eps
end
