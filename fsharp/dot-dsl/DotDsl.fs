module DotDsl
type private S = string
type Child     = Attr of S*S | Node of S*(S*S)list | Edge of S*S*(S*S)list
let graph      = List.sort
let attr k v   = Attr (k, v)
let node k a   = Node (k, a)
let edge l r a = Edge (l, r, a)
let attrs      = List.filter (function Attr _ -> true | _ -> false)
let nodes      = List.filter (function Node _ -> true | _ -> false)
let edges      = List.filter (function Edge _ -> true | _ -> false)