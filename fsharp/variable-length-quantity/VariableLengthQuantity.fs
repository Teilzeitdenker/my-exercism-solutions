module VariableLengthQuantity

let getOneByte (nPart: uint): byte =
    byte (nPart &&& 0x7fu)

let rec getBytes (n: uint): byte seq = 
    if n = 0u then Seq.empty else seq { yield! (getBytes (n >>> 7) |> Seq.map (fun b -> b ||| 0x80uy)) ; yield getOneByte n }

let encode (numbers: uint list): byte list = 
    numbers |> Seq.collect (fun n -> if n = 0u then seq {0uy} else getBytes n) |> Seq.toList  


let getOneNumber (bytes: byte list): uint =
    bytes 
    |> List.rev
    |> List.mapi (fun ind el -> (uint (el &&& 0x7Fuy)) <<< 7*ind )
    |> List.sum

let getNumbers (bytes: byte list): uint seq = 
    let mutable count = 0
    bytes 
    |> List.rev 
    |> List.groupBy (fun h ->  // group them by number of occurrences of a continuation bit 0 from the end of the array
        if (h &&& 0x80uy) = 0uy then 
            count <- count + 1
            count 
        else 
            count)
    |> Seq.map (fun (c, arr) -> arr |> List.rev |> getOneNumber) 
    |> Seq.rev

let decode (bytes: byte list): (uint list) option = 
    if (bytes |> List.last &&& 0x80uy) <> 0uy then None
    else getNumbers bytes |> Seq.toList |> Some

