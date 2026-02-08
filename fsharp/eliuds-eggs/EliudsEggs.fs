module EliudsEggs

let eggCount n = [for i in 0 .. 31 -> (n >>> i) &&& 1] |> List.sum