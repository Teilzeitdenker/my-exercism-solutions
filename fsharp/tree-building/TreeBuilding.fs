// quite fast solution with a mean of 3.063 us (Mine) in contrast to 5.536 us (Baseline)
// and 6.9 KB allocated memory (Mine) in contrast to 13.75 KB (Baseline)

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
    | _ -> false

let children = function 
    | Branch (_, c) -> c
    | _ -> []

let checkForErrors records =  
    match records with
    | []                                                                 -> failwith "input must be nonempty"
    | x :: _  when x.RecordId <> 0 || x.ParentId <> 0                    -> failwith "non-existing or invalid root" 
    | _ :: xs when List.exists (fun r -> r.RecordId <= r.ParentId) xs    -> failwith "non-root record with invalid parent"
    | rs      when (List.last rs).RecordId > (List.length rs - 1)        -> failwith "non-continuous list"
    | _                                                                  -> records

let rec buildHelper parent map =
    match map |> Map.tryFind parent with 
    | None -> Leaf parent 
    | Some children -> Branch (parent, children |> List.map (fun r -> buildHelper r.RecordId map))

let buildTree records =
    records 
    |> List.sortBy (fun r -> r.RecordId)
    |> checkForErrors
    |> List.tail // get rid of the root
    |> List.groupBy (fun r -> r.ParentId)
    |> Map.ofList 
    |> buildHelper 0