module Proverb

let moral word =
    sprintf "And all for the want of a %s." word

let stanza (a , b) =
    sprintf "For want of a %s the %s was lost." a b

let recite (input: string list): string list =
    match input with
    | [] -> []
    | [word] -> [moral word]
    | word :: _  -> (input |> List.pairwise |> List.map stanza) @ [moral word]
                 