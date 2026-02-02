defmodule Poker do
  defmodule HandInfo do
    @ranks "__234567891JQKA"
    @ace_to_5 [14, 5, 4, 3, 2]
    defstruct [:flush, :straight, :ranks]

    def create(deal) do
      {ranks, suits} = deal
        |> Enum.map(fn s -> {@ranks |> String.graphemes |> Enum.find_index(fn c -> String.first(s) == c end), String.last(s)} end)
        |> Enum.sort |> Enum.reverse |> Enum.unzip
      flush = (suits |> Enum.uniq() |> length() == 1)
      straight = (ranks == @ace_to_5) || (ranks |> Enum.uniq |> length == 5 && Enum.at(ranks, 0) - Enum.at(ranks, 4) == 4)
      %HandInfo{flush: flush, straight: straight, ranks: ranks}
    end

    def get_level_and_crucial_ranks(hand_info) do
      {freqs, ranks_by_freqs_desc} = hand_info.ranks
        |> Enum.frequencies
        |> Enum.to_list
        |> Enum.map(fn {a, b} -> {b, a} end)
        |> Enum.sort(:desc)
        |> Enum.unzip
      f0 = freqs |> Enum.at(0)
      f1 = freqs |> Enum.at(1)
      lvl = case {freqs |> length, hand_info.straight, hand_info.flush} do
        {4, _, _}              -> 2 # one pair
        {3, _, _} when f1 == 1 -> 4 # 3 of a kind
        {3, _, _}              -> 3 # two pairs
        {2, _, _} when f0 == 4 -> 8 # 4 of a kind
        {2, _, _}              -> 7 # full house
        {5, true, false}       -> 5 # straight
        {5, false, true}       -> 6 # flush
        {5, true, true}        -> 9 # straight flush
        _                      -> 1 # high card
      end
      {lvl, ranks_by_freqs_desc}
    end
  end

  @spec best_hand(list(list(String.t()))) :: list(list(String.t()))
  def best_hand(deals) do
    deals
      |> Enum.map(fn deal -> {deal, deal |> HandInfo.create |> HandInfo.get_level_and_crucial_ranks} end)
      |> top_group_by(fn {_, {lvl, _}} -> lvl end) # extract hands with highest level
      |> Enum.map(fn {deal, {_, rks}} -> {deal, get_score(rks)} end) # score the crucial ranks
      |> top_group_by(fn {_, scr} -> scr end) # extract highest score
      |> Enum.unzip |> elem(0) # keep only the corresponding deals
  end

  defp top_group_by(e, f) do
    e |> Enum.group_by(fn e -> f.(e) end) |> Enum.to_list() |> Enum.sort(:desc) |> hd |> elem(1)
  end

  # naive scoring of the crucial ranks
  defp get_score(ranks_by_freqs_desc) do
    if ranks_by_freqs_desc == [14, 5, 4, 3, 2] do
      get_score([5, 4, 3, 2, 1])
    else
      ranks_by_freqs_desc |> Enum.reduce(fn el, acc -> acc * 15 + el end)
    end
  end
end
