module BeerSong

open System
open System.Text
open System.Collections.Generic
open System.Linq

let recite (startBottles: int) (takeDown: int) : string list = 
    let numberOfBeers (i : int) : string * string = 
        match i with
        | 0 -> ("No more bottles", "no more bottles")
        | 1 -> ("1 bottle", "1 bottle")
        | n -> ($"{n} bottles", $"{n} bottles")
    let Rhyme (i : int) : string list =
        [$"{fst (numberOfBeers i)} of beer on the wall, {snd (numberOfBeers i)} of beer.";
        match i with 
          | 0 -> "Go to the store and buy some more, 99 bottles of beer on the wall."
          | 1 -> $"Take it down and pass it around, {snd (numberOfBeers 0)} of beer on the wall."
          | n -> $"Take one down and pass it around, {snd (numberOfBeers (n-1))} of beer on the wall."]
    let rec init = function
      | [x] -> []
      | x :: xs -> x :: init xs 
      | [] -> raise <| new ArgumentOutOfRangeException()
    [startBottles..(-1)..(startBottles - takeDown + 1)]
    |> Seq.map(fun i -> Rhyme i)
    |> Seq.map(fun i -> i @ [""] )
    |> Seq.concat 
    |> Seq.toList 
    |> init
    