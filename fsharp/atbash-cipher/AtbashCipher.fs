module AtbashCipher

open System

let cipherMap = Map (['a'..'z'] |> Seq.zip (['a'..'z'] |> Seq.rev))

let atbash (c: char) =
    if Char.IsDigit c then c else cipherMap.[c]

let getEncodedChunks (plain: string) =
    plain |> Seq.filter (Char.IsLetterOrDigit) |> Seq.map (Char.ToLower >> atbash) |> Seq.chunkBySize 5 |> Seq.map String

let encode (str: string) = 
    String.Join(" ", getEncodedChunks str)

let decode (str: string) = 
    String.Join("", getEncodedChunks str)