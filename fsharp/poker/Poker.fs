module Poker

type Hand = {
    ranks    : int array // sorted in descending order
    flush    : bool 
    straight : bool
}

type Level =  
    | HighCard      = 1 
    | Pair          = 2     
    | TwoPair       = 3 
    | ThreeOfAKind  = 4 
    | Straight      = 5 
    | Flush         = 6 
    | FullHouse     = 7 
    | FourOfAKind   = 8 
    | StraightFlush = 9

let ACE_TO_5 = [|14; 5; 4; 3; 2|] // special straight

let createHand (deal : string) = 
    let (rks, sts) =
        deal.Split ' '
        |> Array.map (fun c -> ("__234567891JQKA".IndexOf(c[0]), c[String.length(c) - 1]))
        |> Array.sortByDescending fst
        |> Array.unzip 
    { ranks    = rks
      flush    = sts |> Array.distinct |> Array.length = 1
      straight = (rks = ACE_TO_5) || (rks |> Array.distinct |> Array.length = 5 && rks[0] - rks[4] = 4) }

let getLevelAndCrucialRanks hand = 
    let (ranksByFreqsDesc, freqs) = 
        hand.ranks
        |> Array.countBy id 
        |> Array.sortByDescending (fun (rk, freq) -> (freq, rk))
        |> Array.unzip
    let lvl = 
        match (Array.length freqs, hand.straight, hand.flush) with 
        | (4, _, _)                   -> Level.Pair
        | (3, _, _) when freqs[1] = 1 -> Level.ThreeOfAKind
        | (3, _, _)                   -> Level.TwoPair
        | (2, _, _) when freqs[0] = 4 -> Level.FourOfAKind
        | (2, _, _)                   -> Level.FullHouse 
        | (5, true , false)           -> Level.Straight
        | (5, false, true )           -> Level.Flush
        | (5, true , true )           -> Level.StraightFlush
        | _                           -> Level.HighCard
    (lvl, ranksByFreqsDesc)

let rec getScore ranksByFreqsDesc =
    if ranksByFreqsDesc = ACE_TO_5 then getScore [|5; 4; 3; 2; 1|] // 5 is highest card
    else ranksByFreqsDesc |> Array.reduce (fun acc el -> acc * 14 + el)

let topGroupBy f =
    List.groupBy f >> List.sortByDescending fst >> List.head >> snd

let bestHands (deals : string list) = 
    deals
    |> List.map (fun deal -> (deal, deal |> createHand |> getLevelAndCrucialRanks))
    |> topGroupBy (snd >> fst) // extract the hands with best level
    |> List.map (fun (deal, (_, rks)) -> (deal, getScore rks)) // throw away level and score the crucial ranks
    |> topGroupBy snd // extract the hands with best score
    |> List.unzip |> fst // extract the strings of these deals
