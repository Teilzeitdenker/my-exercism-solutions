module TwoFer

let twoFer (input: string option): string = 
    match input with 
    | Some s -> $"One for {s}, one for me."
    | None -> $"One for you, one for me."