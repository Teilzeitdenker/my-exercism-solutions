module BinarySearch

let rec find (input: int array) (value: int): int option = 
    if input.Length = 0 then
        None
    else
        let middle = input.Length /2
        if input.[middle] = value then 
            Some middle
        elif input.[middle] > value then
            find input.[..(middle-1)] value
        else 
            match find input.[(middle+1)..] value with
            | None -> None
            | Some i -> Some (middle + 1 + i)