module DotDsl

type private Attr = string * string 
type private Node = string * Attr list 
type private Edge = string * string * Attr list 
type Child = AttrChild of Attr | NodeChild of Node | EdgeChild of Edge
type Graph = Child list

let private sortChildren = function
    | AttrChild((n, _))    -> n
    | NodeChild((n, _))    -> n
    | EdgeChild((n, _, _)) -> n

let graph = List.sortBy sortChildren
let attr k v = AttrChild((k, v))
let node k a = NodeChild((k, a))
let edge l r a = EdgeChild((l, r, a))
let attrs = 
    List.choose (fun ch -> 
        match ch with 
        | AttrChild(a) -> AttrChild(a) |> Some
        | _            -> None
    )
let nodes = 
    List.choose (fun ch ->
        match ch with 
        | NodeChild(n) -> NodeChild(n) |> Some 
        | _            -> None
    )
let edges = 
    List.choose (fun ch ->
        match ch with 
        | EdgeChild(e) -> EdgeChild(e) |> Some
        | _            -> None
    )