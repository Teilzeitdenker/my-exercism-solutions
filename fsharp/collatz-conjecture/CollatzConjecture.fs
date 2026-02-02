module CollatzConjecture

let steps (number: int): int option = 
    let rec step number =
        if number = 1 then 
            0
        elif number % 2 = 0 then 
            1 + step (number/2)
        else 
            1 + step (3*number+1)
    if number <= 0 then 
        None
    else 
        step number |> Some
        