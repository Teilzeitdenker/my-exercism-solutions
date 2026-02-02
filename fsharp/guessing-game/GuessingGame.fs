module GuessingGame

let reply (guess: int): string = 
    match guess with
    | 42 -> "Correct"
    | 41 -> "So close"
    | 43 -> "So close"
    | i when i < 42 -> "Too low"
    | i when i > 42 -> "Too high"
    | _ -> "Choose a number"
