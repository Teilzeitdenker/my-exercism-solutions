module BinarySearchTree

type Node = 
    | Null
    | Node of Left:Node * Data:int * Right:Node

let left node  = 
    match node with 
    | Null -> None
    | Node(l, _, _) -> if l = Null then None else Some l

let right node = 
    match node with 
    | Null -> None
    | Node(_, _, r) -> if r = Null then None else Some r 

let data node = 
    match node with 
    | Null -> 0
    | Node(_, d, _) -> d 

let rec create (items:int list) =
    match items with
    | [] -> Null 
    | x::xs  -> 
        let (ys, zs) = xs |> List.partition ((>=) x)
        Node(create ys, x, create zs) 
    

let rec sortedData node = 
    match node with 
    | Null -> []
    | Node(l, d, r) -> sortedData l @ [d] @ sortedData r