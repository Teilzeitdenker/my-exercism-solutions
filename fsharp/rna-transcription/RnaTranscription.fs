module RnaTranscription

let translate = function
    | 'G' -> 'C'
    | 'C' -> 'G'
    | 'T' -> 'A'
    | 'A' -> 'U'
    | _   -> raise <| System.ArgumentException()

let toRna (dna: string): string = 
    dna |> Seq.map translate |> Seq.toArray |> System.String