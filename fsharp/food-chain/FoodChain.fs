module FoodChain
open System

let tiere = ["fly"; "spider"; "bird"; "cat"; "dog"; "goat"; "cow"; "horse"]
let ausrufe = [ "I don't know why she swallowed the fly. Perhaps she'll die.";
         "It wriggled and jiggled and tickled inside her.";
         "How absurd to swallow a bird!";
         "Imagine that, to swallow a cat!";
         "What a hog, to swallow a dog!";
         "Just opened her throat and swallowed a goat!";
         "I don't know how she swallowed a cow!";
         "She's dead, of course!"]

let sonderfall n : string =
    if tiere[n - 1] = "spider" then " that wriggled and jiggled and tickled inside her" else ""

let verse n : string list =
    let ersteZeilen = [$"I know an old lady who swallowed a {tiere[n-1]}."; $"{ausrufe[n-1]}"]
    if n = 8 || n = 1 then ersteZeilen
    else 
        ersteZeilen @
        ([n-1..-1..1] 
        |> List.map (fun i -> $"She swallowed the {tiere[i]} to catch the {tiere[i - 1]}{sonderfall(i)}.")) 
        @ [$"{ausrufe[0]}"]

let recite start stop = ([start..stop - 1] |> List.map verse |> List.map (fun ls -> ls @ [""]) |> List.concat) @ verse stop