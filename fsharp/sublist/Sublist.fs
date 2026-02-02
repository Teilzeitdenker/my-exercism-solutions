module Sublist

type SublistType = Equal | Sublist | Superlist | Unequal

let sublist xs ys = 
    match (xs |> List.length, ys |> List.length) with 
    | (0, 0) -> Equal
    | (0, _) -> Sublist 
    | (_, 0) -> Superlist 
    | (m, n) when m > n -> if xs |> List.windowed n |> List.exists (fun v -> v = ys) then Superlist else Unequal
    | (m, n) when m < n -> if ys |> List.windowed m |> List.exists (fun v -> v = xs) then Sublist else Unequal
    | _ -> if xs = ys then Equal else Unequal
