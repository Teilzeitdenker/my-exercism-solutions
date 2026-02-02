module Anagram

open System

let getCharSet word = 
    word |> Seq.countBy (fun c -> Char.ToLower c) |> Set.ofSeq

let getLower word =
    word |> Seq.map (Char.ToLower) |> Seq.toArray |> String

let isAnagram targetSet source =
    getCharSet source = targetSet

let findAnagrams sources target = 
    let targetSet = getCharSet target
    let isOtherWord source = (target |> getLower) <> (source |> getLower)
    sources |> List.filter isOtherWord |> List.filter (isAnagram targetSet) 