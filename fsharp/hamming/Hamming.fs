module Hamming

let distance (strand1: string) (strand2: string): int option = 
    let arr1 = strand1.ToCharArray()
    let arr2 = strand2.ToCharArray()
    match arr1.Length = arr2.Length with
    | false -> None
    | true -> 
        Array.zip arr1 arr2 
        |> Array.filter (fun (a, b) -> a <> b) 
        |> Array.length 
        |> Some