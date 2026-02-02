module Proverb

let recite (input: string list): string list = 
    let rec tailRecite head rest ls =
        match rest with
        | [] -> ls
        | [one] -> ls @ [sprintf "And all for the want of a %s." head]
        | first :: second :: rest -> ls @ [sprintf "For want of a %s the %s was lost." first second] |> tailRecite head (second::rest) 
    match input with
    | [] -> []
    | [word] -> [sprintf "And all for the want of a %s." word]
    | head :: second :: rest -> tailRecite head (second::rest) [sprintf "For want of a %s the %s was lost." head second]   