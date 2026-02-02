// This is the file you need to modify for your own solution.
// The unit tests will use this code, and it will be used by the benchmark tests
// for the "Mine" row of the summary table.

// Remember to not only run the unit tests for this exercise, but also the
// benchmark tests using `dotnet run -c Release`.
// Please refer to the instructions for more information about the benchmark tests.

module TreeBuilding

open TreeBuildingTypes

type Tree =
    | Leaf of int
    | Branch of int * Tree list

let recordId = function
    | Leaf id -> id
    | Branch (id, _) -> id
    

let isBranch = function 
    | Branch _ -> true
    | Leaf _ -> false

let children = function 
    | Branch (_, c) -> c
    | Leaf _ -> []

let buildTree records =
    if List.isEmpty records then failwith "Empty input"

    let checkRecordTuple (p, r) = 
        if r > 0 then 
            if p >= r then failwith "Non-root record with invalid parent"
            (p, r)
        else if r = 0 then 
            if p <> 0 then failwith "Root node is invalid" 
            (p, r)
        else failwith "Node with negative record ID"

    let checkIndex (i, (p, r)) = 
        if i <> r then failwith "Non-continuous list" 
        (p, r)

    let parentChildrenMap =
        records 
        |> List.map (fun r -> (r.ParentId, r.RecordId) ) 
        |> List.sortBy snd
        |> List.map checkRecordTuple
        |> List.indexed 
        |> List.map checkIndex
        |> List.tail
        |> List.groupBy fst 
        |> List.map (fun (x, y) -> (x, List.map snd y))
        |> Map.ofSeq 

    let rec buildHelper parent =
        match Map.tryFind parent parentChildrenMap with
        | Some children -> Branch (parent, children |> List.map buildHelper)
        | _             -> Leaf parent

    buildHelper 0