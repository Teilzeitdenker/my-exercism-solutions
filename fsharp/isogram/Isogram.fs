module Isogram

let isIsogram str = 
    let letters : char seq = str |> Seq.filter (System.Char.IsLetter) |> Seq.map (System.Char.ToLower)
    Seq.length letters = (letters |> Seq.distinct |> Seq.length)