module ScaleGenerator

let sharps = ["A"; "A#"; "B"; "C"; "C#"; "D"; "D#"; "E"; "F"; "F#"; "G"; "G#"; "A"; "A#"; "B"; "C"; "C#"; "D"; "D#"; "E"; "F"; "F#"; "G"; "G#"]
let flats =  ["A"; "Bb"; "B"; "C"; "Db"; "D"; "Eb"; "E"; "F"; "Gb"; "G"; "Ab"; "A"; "Bb"; "B"; "C"; "Db"; "D"; "Eb"; "E"; "F"; "Gb"; "G"; "Ab"]
let sharpTonics = ["C"; "G"; "D"; "A"; "E"; "B"; "F#"; "a"; "e"; "b"; "f#"; "c#"; "g#"; "d#"]

let toInvariant tonic =
    if tonic |> String.length = 1 then tonic.ToUpper()
    else tonic.[0].ToString().ToUpper() + tonic.[1].ToString()

let chromatic tonic = 
    let scale =
        if sharpTonics |> List.contains tonic then 
            sharps 
        else
            flats
    scale |> List.skipWhile ((<>) (toInvariant tonic)) |> List.take 12

let interval (intervals: string) tonic = 
    let filterPattern = 
        intervals 
        |> Seq.collect (fun c -> 
            match c with
            | 'm' -> [true]
            | 'M' -> [true; false]
            | 'A' -> [true; false; false]
            | _   -> raise <| System.ArgumentException("No such interval") )
        |> Seq.toList
    chromatic tonic 
    |> List.zip filterPattern
    |> List.filter fst
    |> List.map snd
