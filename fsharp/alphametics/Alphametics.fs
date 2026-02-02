module Alphametics

open System
open FParsec

// parsing types and functions using the FParsec package
// see http://www.quanttec.com/fparsec/reference/parser-overview.html 
type private UserState = unit
type private Parser<'t> = Parser<'t, UserState>

let private upperLetter: Parser<_> = anyOf (String [|'A'..'Z'|])
let private word:        Parser<_> = spaces >>. many1Chars upperLetter .>> spaces
let private summands:    Parser<_> = sepBy1 word (pstring "+") .>> (pstring "==")
let private fullParser:  Parser<_> = summands .>>. word .>> eof

let private parse input =
    match run fullParser input with
    | Failure(_, _, _)   -> None
    | Success(res, _, _) -> Some res 

// functions for calculating permutations and combinations (Haskell-like)
let private cons x = function 
    | [] -> [x]
    | xs -> x::xs

let rec private inserts x = function
    | []           -> [ [x] ]
    | (y::ys) as l -> (x::l)::(List.map (cons y) (inserts x ys))

let rec private permutations = function
    | []    -> seq [ [] ]
    | x::xs -> Seq.concat (Seq.map (inserts x) (permutations xs))

let private n_choose_k arr k =
    let len = Array.length arr
    let rec choose lo x =
        match x with
        | 0 -> [[]]
        | i -> [ for j in lo..(len-1) do
                   for ks in choose (j+1) (i-1) do
                       yield arr.[j]::ks ]
    choose 0 k

// processing a word (result word starting with a placeValue of -1, all summands with +1)
let private proc (placeValue, oldMap) word  =
    // fold this over every character of the word
    let folder (map: Map<char,int>, placeValue) ch =
        let prevValue = 
            match map.TryFind(ch) with
            | Some count -> count
            | None       -> 0 
        (map.Add(ch, prevValue + placeValue), placeValue * 10)
    let newMap = 
        word 
        |> Seq.fold folder (oldMap, placeValue) 
        |> fst
    (placeValue, newMap)

let private letterPlaceValuesAndNonZeroSet summands result =
    let preMap = 
        summands
        |> Seq.map Seq.rev 
        |> Seq.fold proc (1, Map.empty) 
        |> snd
    let fullMap =  
        result 
        |> Seq.rev 
        |> proc (-1, preMap) 
        |> snd
    let nonZeroSet = 
        result::summands 
        |> Seq.map Seq.head 
        |> set
    (fullMap, nonZeroSet)

let private compute summands result = 
    let (fullMap, nonZeroSet) = letterPlaceValuesAndNonZeroSet summands result
    let allPerms = Seq.collect permutations (n_choose_k [|0..9|] fullMap.Count)
    let fullList = fullMap |> Map.toList
    // multiply the digit corresponding to the letter with the place value from the map
    let folder acc el1 (_,el2) =
        acc + el1 * el2
    // helper function to check if a permutation is a solution to the problem
    let isSolution perm =
        let sum = Seq.fold2 folder 0 perm fullList
        let zeroIdx = List.tryFindIndex(fun p -> p = 0) perm
        match zeroIdx with
        | Some t -> not (nonZeroSet.Contains((fst fullList.[t]))) && sum = 0
        | None   -> sum = 0
    // return the solution as a Map<char, int> behind a Some
    match allPerms |> Seq.tryFind isSolution with 
    | Some perm -> 
        perm 
        |> List.zip fullList 
        |> List.map (fun ((c,_),v) -> (c,v)) 
        |> Map 
        |> Some
    | None      -> None

let solve input = 
    match parse input with
    | Some (summands, result) -> compute summands result
    | None                    -> None
    