module RailFenceCipher

open System

let upAndDown rails = Seq.initInfinite (fun _ -> Seq.append (seq [1..rails - 1]) (seq [2..rails] |> Seq.rev)) |> Seq.concat

let encode rails = 
    Seq.zip (upAndDown rails) 
    >> Seq.groupBy fst
    >> Seq.map (snd >> Seq.map snd >> Seq.toArray >> String)
    >> String.concat ""


let decode rails message = 
     upAndDown rails // start with the infinite rail sequence
    |> Seq.zip <| seq {1..(message |> Seq.length)} // zip in the possible position numbers
    |> Seq.sort // sort these tuples by rail number
    |> Seq.map snd // throw away the rail number
    |> Seq.zip <| seq message // zip in the rail-sorted chars of the encoded message from behind
    |> Seq.sort // and sort the chars back into their original positions
    |> Seq.map snd // throw away the position
    |> Seq.toArray
    |> String 
