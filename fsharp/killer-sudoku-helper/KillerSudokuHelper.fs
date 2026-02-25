module KillerSudokuHelper

let rec private allSortedCombs numbers size =
    if size = 1 then numbers |> List.map List.singleton
    else numbers |> List.collect (fun n -> 
            allSortedCombs (numbers |> List.filter ((<) n)) (size - 1)
            |> List.map (fun c -> n :: c))
           
let combinations exclude size sum : int list list =
    allSortedCombs ([1..9] |> List.except exclude) size |> List.filter (List.sum >> (=) sum)
