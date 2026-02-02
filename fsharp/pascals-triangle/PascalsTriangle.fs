module PascalsTriangle

let rec rows numberOfRows : int list list = 
    match numberOfRows with
    | 0 -> []
    | 1 -> [[1]]
    | n ->  let before = rows (numberOfRows - 1)
            before @ [(1 :: ( before |> List.last |> List.pairwise |> List.map (fun a -> fst a + snd a) ) @ [1])]
           