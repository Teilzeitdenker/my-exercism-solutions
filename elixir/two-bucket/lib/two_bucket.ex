defmodule TwoBucket do
  defstruct [:bucket_one, :bucket_two, :moves]
  @type t :: %TwoBucket{bucket_one: integer, bucket_two: integer, moves: integer}

  @doc """
  Find the quickest way to fill a bucket with some amount of water from two buckets of specific sizes.
  """
  @spec measure(
          size_one :: integer,
          size_two :: integer,
          goal :: integer,
          start_bucket :: :one | :two
        ) :: {:ok, TwoBucket.t()} | {:error, :impossible}
  def measure(size_one, size_two, goal, start_bucket) do
    initial_state   = if start_bucket == :one, do: {size_one, 0}, else: {0, size_two}
    forbidden_state = if start_bucket == :one, do: {0, size_two}, else: {size_one, 0}
    all_reachable_states =
      {size_one, size_two, [initial_state], [forbidden_state]}
      |> Stream.unfold(&get_next_states/1)
    result = all_reachable_states
      |> Stream.with_index(1)
      |> Stream.map(&try_two_bucket_type_from(&1, goal))
      |> Stream.drop_while(& &1 == nil)
      |> Enum.take(1) # use Enum to run the stream
    case result do
      [] -> {:error, :impossible}
      _  -> {:ok   ,  hd(result)}
    end
  end

  defp apply_all_moves(size_one, size_two, {a, b}) do
    pour_left = min(size_one - a, b)
    pour_right = min(size_two - b, a)
    [
      {size_one, b},                     # fill
      {a, size_two},
      {0, b},                            # empty
      {a, 0},
      {a + pour_left, b - pour_left},    # pour
      {a - pour_right, b + pour_right}
    ]
  end

  defp get_next_states({size_one, size_two, states, explored}) do
    next_states = states
      |> Enum.flat_map(&apply_all_moves(size_one, size_two, &1))
      |> Enum.reject(&Enum.member?(explored, &1))
      |> Enum.dedup
    case next_states do
      [] -> nil # terminates Stream.unfold
      _  -> {states, {size_one, size_two, next_states, Enum.concat(explored, next_states)}}
    end
  end

  defp try_two_bucket_type_from({ls, idx}, goal) do
    case {ls |> Enum.find(&elem(&1, 0) == goal), ls |> Enum.find(&elem(&1, 1) == goal)} do
      {{_, other}, _         } -> %TwoBucket{bucket_one: goal, bucket_two: other, moves: idx}
      { _        , {other, _}} -> %TwoBucket{bucket_one: other, bucket_two: goal, moves: idx}
      _                        -> nil
    end
  end
end
