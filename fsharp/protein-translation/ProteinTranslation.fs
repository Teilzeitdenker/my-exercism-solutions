module ProteinTranslation

open System

let toProtein(actualCodon : string) =
    match actualCodon with
        | "AUG"                         -> "Methionine"
        | "UUU" | "UUC"                 -> "Phenylalanine"
        | "UUA" | "UUG"                 -> "Leucine"
        | "UCU" | "UCC" | "UCA" | "UCG" -> "Serine"
        | "UAU" | "UAC"                 -> "Tyrosine"
        | "UGU" | "UGC"                 -> "Cysteine"
        | "UGG"                         -> "Tryptophan"
        | "UAA" | "UAG" | "UGA"         -> "STOP"
        | _                             -> raise <| ArgumentOutOfRangeException()   

let proteins (rna : string) = 
    let numCodons = rna.Length / 3
    rna 
    |> Seq.splitInto numCodons 
    |> Seq.map (String >> toProtein) 
    |> Seq.takeWhile (fun x -> x <> "STOP") 
    |> Seq.toList
