module AffineCipher

open System

let cands = [1..2..11] @ [15..2..25]

let isNotValid (a: int) = 
    not (List.contains (a%26) cands)

let modInverse26 (a:int) =
    cands |> List.filter (fun i -> ((a%26) * (i%26)) % 26 = 1) |> List.exactlyOne

let letterToInt (c: char) =
    int (Char.ToLower(c)) - int 'a'

let rec intToLetter (i: int) =
    if i < 0 then 
        intToLetter (i + 26)
    else 
        char ((i%26) + int 'a')

let decode a b cipheredText = 
    if isNotValid a then 
        raise <| System.ArgumentException()
    else
        let affineDecode (c:char) =
            let modInv = modInverse26 a
            let y = letterToInt c
            intToLetter (modInv * (y - b))
        cipheredText 
        |> Seq.filter (Char.IsLetterOrDigit) 
        |> Seq.map (fun c -> if Char.IsDigit(c) then c else affineDecode c)
        |> Seq.toArray
        |> System.String


let encode a b plainText = 
    if isNotValid a then 
        raise <| System.ArgumentException()
    else
        let affineEncode (c:char) =
            let x = letterToInt c
            intToLetter (a * x + b)
        let listOf5s = 
            plainText 
            |> Seq.filter (Char.IsLetterOrDigit) 
            |> Seq.map (fun c -> if Char.IsDigit(c) then c else affineEncode c)
            |> Seq.chunkBySize 5 
            |> Seq.map (fun arr -> String(arr)) |> Seq.toList
        String.Join(" ", listOf5s)
