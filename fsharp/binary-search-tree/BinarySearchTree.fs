module BinarySearchTree

type Node = 
    | Null
    | Node of Node * int * Node

let left = function
    | Null | Node(Null, _, _) -> None
    | Node(l, _, _) -> Some l

let right = function 
    | Null | Node(_, _, Null) -> None
    | Node(_, _, r) -> Some r 

let data = function
    | Null -> failwith "No data for empty tree"
    | Node(_, d, _) -> d 

let rec create = function
    | [] -> Null 
    | x::xs  -> 
        let (ys, zs) = xs |> List.partition ((>=) x)
        Node(create ys, x, create zs) 
    
let rec sortedData = function
    | Null -> []
    | Node(l, d, r) -> sortedData l @ [d] @ sortedData r