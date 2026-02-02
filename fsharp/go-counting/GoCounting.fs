module GoCounting

type Owner = White | Black | None

type private Board = string list

let private width (board: Board) = board[0] |> String.length

let private height (board: Board) = board |> List.length

let private onBoard (board: Board) (x, y) = 
    (x >= 0) && (x < width board) && (y >= 0) && (y < height board)

let private neighbors (board: Board) (x, y) = 
    [| (-1, 0); (1, 0); (0, -1); (0, 1) |]
    |> Array.map (fun (dx, dy) -> (x + dx, y + dy)) 
    |> Array.filter (onBoard board)

let private ownerOf (board: Board) (x, y) = 
    match board[y][x] with 
    | 'W' -> White 
    | 'B' -> Black
    | _   -> None

let private emptyFields (board: Board) = 
    [for x in 0..(width board - 1) do 
        for y in 0..(height board - 1) do 
            if ownerOf board (x, y) = None then yield (x, y)]

let public territory board position : (Owner * (int * int) list) option = 
    if not (onBoard board position) then 
        Option.None 
    elif ownerOf board position <> None then 
        Some (None, [])
    else 
        let mutable fields = [position] |> Set.ofList
        let mutable edges = [] |> Set.ofList
        let ngbs = neighbors board position 
        let updateFieldsAndEdges p =
            match ownerOf board p with 
            | None -> fields <- fields |> Set.add p 
            | _    -> edges  <- edges  |> Set.add p
        let rec loop actualNgbs =
            if actualNgbs |> Array.isEmpty then ()
            else
                actualNgbs |> Array.map updateFieldsAndEdges |> ignore 
                loop (actualNgbs // get new neighbors and recurse
                |> Array.filter (fun p -> ownerOf board p = None)
                |> Array.collect (neighbors board)
                |> Array.filter (fun p -> not (Set.contains p fields || Set.contains p edges)))
        loop ngbs |> ignore
        let edgeOwners = edges |> Seq.map (ownerOf board) |> Seq.distinct
        if Seq.length edgeOwners = 1 then Some (Seq.head edgeOwners, Set.toList fields)
        else  Some (None, Set.toList fields) 

let public territories board : Map<Owner, (int * int) list> = 
    let resultMap = [(Black, []); (White, []); (None, [])] |> Map.ofList
    let empties = emptyFields board |> Set.ofList
    let rec loop emptyRest acc = 
        if emptyRest |> Set.isEmpty then acc
        else 
            let pos = emptyRest |> Seq.head 
            match territory board pos with
            | Some (owner, fields) -> 
                let newEntry =  Map.find owner acc |> List.append fields |> List.sort
                loop (Set.ofList fields |> Set.difference emptyRest) (Map.add owner newEntry acc)
            | _                    -> failwith "unreachable"
    loop empties resultMap