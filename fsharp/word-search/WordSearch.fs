module WordSearch

let directions = seq { (1,0);(1,1);(0,1);(-1,1);(-1,0);(-1,-1);(0,-1);(1,-1) }

// helper functions for addition and scalar multiplication of tuples
let addTuples (a1,b1) (a2,b2) = (a1+a2,b1+b2)
let scalarMult k (a,b) = (k*a,k*b)

// no Map.merge in the F# Map module, what a shame...
// Imitating the pattern of Map.merge in Elixir,
// we take the first map a as a starting point and fold over every entry (k, v) of the second map b
// while resolving merge conflicts by application of a function f with parameters k, value_in_a and value_in_b
let merge (a : Map<'a, 'b>) (b : Map<'a, 'b>) (f : 'a -> 'b -> 'b -> 'b) =
    (a, b) ||> Map.fold (fun acc k vb ->
        match Map.tryFind k acc with
        | Some va -> acc |> Map.add k (f k va vb)
        | None -> acc |> Map.add k vb)

// collects for every given character in the grid (the key of the map) a sequence of its index positions (the value of the map) 
let gridToMap (grid : string list) : Map<char,(int * int) seq> =
    grid 
    |> Seq.indexed 
    |> Seq.map (fun (i, s) -> 
        s 
        |> Seq.indexed 
        |> Seq.map (fun (j, c) -> 
            seq { (c, seq { (j+1, i+1) }) } |> Map.ofSeq 
        )
        |> Seq.fold (fun acc m -> merge acc m (fun _ v1 v2 -> Seq.append v1 v2)) Map.empty
     )
     |> Seq.fold (fun acc m -> merge acc m (fun _ v1 v2 -> Seq.append v1 v2)) Map.empty

// fetches all possible starting positions from the gridMap by looking at the first character of the word
// then enriches every starting position with all possible 8 directions and then uses checkDir from index 1 
// to filter out the successful combinations, takes the first successful result (if any), 
// calculates the end position of the word and wraps start and end position together in an option
let checkWord (gridMap : Map<char,(int * int) seq>) (word : string) : string * Option<((int * int) * (int * int))> = 
    let rec checkDir start dir index = 
        if index = word.Length then true 
        else 
            let position = addTuples start (scalarMult index dir) 
            if gridMap[word[index]] |> Seq.contains position then checkDir start dir (index + 1) else false 

    if word |> Seq.exists(fun ch -> Map.containsKey ch gridMap |> not) then (word, None)
    else 
        let results = 
            gridMap[word[0]] 
            |> Seq.collect (fun start -> directions |> Seq.map (fun dir -> (start, dir)))
            |> Seq.filter (fun (start, dir) -> checkDir start dir 1)
        match results |> Seq.tryHead with 
            | None -> (word, None)
            | Some (start, dir) -> 
                let endpos = addTuples start (scalarMult (word.Length - 1) dir) 
                (word, Some (start, endpos))
      
let search grid wordsToSearchFor = 
    let gridMap = gridToMap grid 
    wordsToSearchFor |> Seq.map (fun word -> checkWord gridMap word) |> Map.ofSeq
