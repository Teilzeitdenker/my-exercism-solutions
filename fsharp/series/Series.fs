module Series

open System

let slices str length: option<string list> =
    let numDigs = str |> String.length
    if str = String.Empty || numDigs < length || length <= 0 then 
        None
    else
        str |> Seq.windowed length |> Seq.map String |> Seq.toList |> Some