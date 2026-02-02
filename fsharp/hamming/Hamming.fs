module Hamming

let distance (s1: string) (s2: string): int option = 
    if s1.Length <> s2.Length then None
    else Seq.zip s1 s2 |> Seq.map (fun (a, b) -> if a = b then 0 else 1) |> Seq.sum |> Some