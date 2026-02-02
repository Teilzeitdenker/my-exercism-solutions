module AllYourBase

let rebase digits inputBase outputBase = 
    match (inputBase, outputBase) with
    | (i, _) when i <= 1 -> None
    | (_, o) when o <= 1 -> None
    | (i, o) -> 
        match digits with 
        | ds when ds |> List.filter (fun d -> d >= i || d < 0) |> List.length > 0 -> None
        | ds -> 
            let number = ds |> List.rev |> List.mapi (fun i d -> d * pown inputBase i) |> List.sum
            let rec innerRebase num = 
                if num < outputBase then [num]
                else (innerRebase (num/outputBase)) @ [num % outputBase] 
            innerRebase number |> Some
            
