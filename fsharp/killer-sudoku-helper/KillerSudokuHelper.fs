module KillerSudokuHelper

let rec private combinationsWithoutRepetition numbers size =
    match size with
    | 1 -> numbers |> List.map (fun n -> [n])
    | s -> numbers |> List.collect (fun n -> 
           combinationsWithoutRepetition (numbers |> List.filter (fun el -> el > n)) (s - 1)
           |> List.map (fun c -> n :: c))
           
let combinations exclude size sum : int list list =
    combinationsWithoutRepetition ([1..9] |> List.except exclude) size
    |> List.filter (fun c -> List.sum c = sum)
