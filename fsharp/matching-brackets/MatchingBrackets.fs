module MatchingBrackets

let fillBracketStack (l: char list option) (c: char): char list option =
    let opened = ['(';'[';'{']
    let closed = [')';']';'}']
    if List.contains c opened then 
        Option.map (fun ls -> c :: ls) l
    elif List.contains c closed then
        match l with 
        | None -> None
        | Some [] -> None
        | Some (hd :: tl) -> 
            if List.findIndex ((=) c) closed = List.findIndex ((=) hd) opened then 
                Some tl 
            else 
                None
    else
        l

let isPaired (input: string): bool = 
    match Seq.fold fillBracketStack (Some []) input with
    | None -> false
    | Some ls -> ls |> List.isEmpty