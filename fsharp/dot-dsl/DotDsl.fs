module DotDsl

type private Attr = string * string 
type private Node = string * Attr list 
type private Edge = string * string * Attr list 
type Child = AttrChild of Attr | NodeChild of Node | EdgeChild of Edge
type Graph = Child list

let graph = List.sort
let attr k v = AttrChild (k, v)
let node k a = NodeChild (k, a)
let edge l r a = EdgeChild (l, r, a)
let attrs = List.filter (function AttrChild _ -> true | _ -> false)
let nodes = List.filter (function NodeChild _ -> true | _ -> false)
let edges = List.filter (function EdgeChild _ -> true | _ -> false)