module Change

let cons x y = x :: y

let appendCoin map amount coin = 
    map |> Map.find (amount - coin) 
    |> Option.map (cons coin)

let takeShortestList = function
    | [] -> None 
    | ls -> ls |> List.minBy List.length |> Some

let getNextCoinList coins map amount = 
    coins 
    |> List.filter ((>=) amount) 
    |> List.choose (appendCoin map amount)
    |> takeShortestList

let updateMap coins map amount = 
    Map.add amount (getNextCoinList coins map amount) map

let findFewestCoins coins target =
    if target < 0 then None else 
    [1..target]
    |> Seq.fold (updateMap coins) (Map.ofList [(0, Some [])])
    |> Map.find target