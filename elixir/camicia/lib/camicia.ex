defmodule Camicia do
  @doc """
    Simulate a card game between two players.
    Each player has a deck of cards represented as a list of strings.
    Returns a tuple with the result of the game:
    - `{:finished, cards, tricks}` if the game finishes with a winner
    - `{:loop, cards, tricks}` if the game enters a loop
    `cards` is the number of cards played.
    `tricks` is the number of central piles collected.

    ## Examples

      iex> Camicia.simulate(["2"], ["3"])
      {:finished, 2, 1}

      iex> Camicia.simulate(["J", "2", "3"], ["4", "J", "5"])
      {:loop, 8, 3}
  """
  @card_encodings %{"J" => 1, "Q" => 2, "K" => 3, "A" => 4}

  @spec simulate(list(String.t()), list(String.t())) ::
          {:finished | :loop, non_neg_integer(), non_neg_integer()}
  def simulate(player_a, player_b) do
    hand_a = Enum.map(player_a, &encode_card/1)
    hand_b = Enum.map(player_b, &encode_card/1)
    pile = []
    player = 1 # A is 1, B is -1, switch players by multiplying -1
    tricks = 0
    turns = 0
    states = MapSet.new([get_state_signature(hand_a, hand_b)])
    finite = true

    do_sim(%{hand_a: hand_a, hand_b: hand_b, pile: pile, player: player,
      tricks: tricks, turns: turns, states: states, finite: finite})
  end

  defp do_sim(%{hand_a: [], tricks: tr, turns: tu}), do: {:finished, tu, tr}
  defp do_sim(%{hand_b: [], tricks: tr, turns: tu}), do: {:finished, tu, tr}
  defp do_sim(%{finite: false, tricks: tr, turns: tu}), do: {:loop, tu, tr}
  defp do_sim(state), do: state |> play_round() |> do_sim()

  defp play_round(%{hand_a: [], hand_b: _hand_b} = state), do: state
  defp play_round(%{hand_a: _hand_a, hand_b: []} = state), do: state
  defp play_round(state) do
    %{hand_a: hand_a, hand_b: hand_b, pile: pile, player: player,
      tricks: tricks, turns: turns, states: states, finite: finite} = state

    {new_hand_a, new_hand_b, new_pile, new_player, new_turns} =
      play_cards(hand_a, hand_b, pile, player, turns, 1, false)

    winner = new_player * -1
    {collected_hand_a, collected_hand_b} =
      case winner do
        1 -> {new_hand_a ++ Enum.reverse(new_pile), new_hand_b}
        _ -> {new_hand_a, new_hand_b ++ Enum.reverse(new_pile)}
      end

    new_signature = get_state_signature(collected_hand_a, collected_hand_b)

    {new_states, new_finite} =
      case MapSet.member?(states, new_signature) do
        true -> {states, false}
        false -> {MapSet.put(states, new_signature), finite}
      end

    %{hand_a: collected_hand_a, hand_b: collected_hand_b, pile: [], player: winner,
      tricks: tricks + 1, turns: new_turns, states: new_states, finite: new_finite}
  end

  # variables are (hand_a, hand_b, pile, player, turns, cards_to_play, battle?)
  defp play_cards(a, b, p, pl, tu, ctp, _) when ctp <= 0, do: {a, b, p, pl, tu}
  defp play_cards([], b, p, 1, tu, _, _), do: {[], b, p, 1, tu}
  defp play_cards(a, [], p, -1, tu, _, _), do: {a, [], p, -1, tu}
  defp play_cards([c | a], b, p, 1, tu, ctp, b?), do: process_card(c, a, b, p, 1, tu, ctp, b?)
  defp play_cards(a, [c | b], p, -1, tu, ctp, b?), do: process_card(c, a, b, p, -1, tu, ctp, b?)

  defp process_card(card, hand_a, hand_b, pile, player, turns, cards_to_play, battle?) do
    new_pile = [card | pile]
    new_turns = turns + 1
    next_player = -player
    case card do
      0 ->
        if battle? do
          play_cards(hand_a, hand_b, new_pile, player, new_turns, cards_to_play - 1, true)
        else
          play_cards(hand_a, hand_b, new_pile, next_player, new_turns, 1, false)
        end
      value ->
        play_cards(hand_a, hand_b, new_pile, next_player, new_turns, value, true)
    end
  end

  defp encode_card(card), do: Map.get(@card_encodings, card, 0)
  defp get_state_signature(a, b), do: "#{Enum.join(a)}|#{Enum.join(b)}"
end
