module BookStore

let numBooksSorted books = 
    books |> List.countBy id |> List.map snd |> List.sort

let rec getStacks (acc: int list) (sorted: int list) = 
    match sorted with
    | [] -> acc
    | el :: _  -> 
        let rest = 
            sorted 
            |> List.skipWhile (fun e -> e = el) 
            |> List.map (fun e -> e - el)
        getStacks (acc @ List.replicate el (List.length sorted)) rest

let priceForStackSize ((size, anz): int*int): decimal = 
    match size with
    | 1 -> 8.0m * (anz |> decimal)
    | 2 -> 15.2m * (anz |> decimal)
    | 3 -> 21.6m * (anz |> decimal)
    | 4 -> 25.6m * (anz |> decimal)
    | 5 -> 30.0m * (anz |> decimal)
    | _ -> 0.0m

let getNiceStackEntry grouped size =
    match grouped |> List.tryFind (fun (el, _) -> el = size) with
    | Some entry -> entry 
    | None          -> (size, 0)


let eliminatePairsOf5And3 groupedStacks = 
    let fives = getNiceStackEntry groupedStacks 5 |> snd
    let threes = getNiceStackEntry groupedStacks 3 |> snd 
    let eliminate = min fives threes
    let additionalFours = 2 * eliminate
    [groupedStacks.[0]; groupedStacks.[1]; (3,snd groupedStacks.[2] - eliminate);(4, snd groupedStacks.[3] + additionalFours); (5, snd groupedStacks.[4] - eliminate)]

let calculateLowestPrice groupedStacks =
    List.map (getNiceStackEntry groupedStacks) [1..5] 
    |> eliminatePairsOf5And3 
    |> List.sumBy priceForStackSize

let total books = 
    books 
    |> numBooksSorted 
    |> getStacks []
    |> List.countBy id
    |> calculateLowestPrice