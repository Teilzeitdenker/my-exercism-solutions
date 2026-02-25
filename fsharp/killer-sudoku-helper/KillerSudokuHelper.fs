module KillerSudokuHelper

let rec private allSortedCombs numbers size =
    if size = 1 then // put each number in its own list
        numbers |> List.map List.singleton
    else // start at each number and find all combinations of the remaining numbers
        numbers |> List.collect (fun n -> 
            // remember that (<) n is short for fun x -> (<) n x = n < x, so we 
            // look at all greater numbers to avoid repetitions in the combinations
            allSortedCombs (numbers |> List.filter ((<) n)) (size - 1)
            // prepend n to each of these combinations 
            |> List.map (fun c -> n :: c))
           
let combinations exclude size sum : int list list =
    allSortedCombs ([1..9] |> List.except exclude) size 
    // pull out the combinations with the right sum 
    |> List.filter (List.sum >> (=) sum)
