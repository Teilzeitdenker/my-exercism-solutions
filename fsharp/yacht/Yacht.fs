module Yacht

type Category = 
    | Ones
    | Twos
    | Threes
    | Fours
    | Fives
    | Sixes
    | FullHouse
    | FourOfAKind
    | LittleStraight
    | BigStraight
    | Choice
    | Yacht

type Die =
    | One   = 1
    | Two   = 2
    | Three = 3
    | Four  = 4
    | Five  = 5
    | Six   = 6

let frequencies (dice : Die list) : Map<Die, int> =
    dice |> Seq.groupBy id |> Seq.map (fun (die, ls) -> (die, Seq.length ls)) |> Map.ofSeq

let freqVals (dice : Die list) : int list = 
    frequencies dice |> Map.values |> Seq.sort |> Seq.toList

let score (category : Category) (dice : Die list) : int = 
    match category with 
    | Ones           -> (dice |> Seq.filter ((=) Die.One)   |> Seq.length) * 1
    | Twos           -> (dice |> Seq.filter ((=) Die.Two)   |> Seq.length) * 2
    | Threes         -> (dice |> Seq.filter ((=) Die.Three) |> Seq.length) * 3
    | Fours          -> (dice |> Seq.filter ((=) Die.Four)  |> Seq.length) * 4
    | Fives          -> (dice |> Seq.filter ((=) Die.Five)  |> Seq.length) * 5
    | Sixes          -> (dice |> Seq.filter ((=) Die.Six)   |> Seq.length) * 6
    | FullHouse      -> if freqVals dice = [2; 3] then dice |> Seq.map int |> Seq.sum else 0
    | FourOfAKind    -> frequencies dice |> Seq.map (fun kvp -> if [4; 5] |> Seq.contains kvp.Value then 4 * int kvp.Key else 0) |> Seq.sum
    | LittleStraight -> if dice |> List.map int |> List.sort = [1..5] then 30 else 0
    | BigStraight    -> if dice |> List.map int |> List.sort = [2..6] then 30 else 0
    | Choice         -> dice |> Seq.map int |> Seq.sum 
    | Yacht          -> if dice |> List.distinct |> List.length = 1 then 50 else 0