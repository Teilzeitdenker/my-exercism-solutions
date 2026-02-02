module Dominoes
    
let canChain input = 
    let rec loop st en ls = 
        match ls with 
        | [(a, b)] -> (en = a && st = b) || (en = b && st = a)
        | _ ->
            let poss = ls |> List.filter (fun (a, b) -> a = en || b = en)
            if List.isEmpty poss then false 
            else 
                let mutable res = false
                for el in poss do
                    if res then 
                        res <- res
                    else 
                        res <- loop st (if (snd el) = en then fst el else snd el) (List.removeAt (List.findIndex ((=) el) ls) ls)
                res        
    match input with
    | [] -> true
    | [(a, b)] -> a = b
    | (a, b) :: rest -> loop a b rest