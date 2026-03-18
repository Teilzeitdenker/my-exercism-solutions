module Satellite

type Tree =
    | Empty
    | Node of value: string * left: Tree * right: Tree

let private notSameLength a b = List.length a <> List.length b
let private notSameElements a b = Set.ofList a <> Set.ofList b
let private notAllDistinct a = Set.ofList a |> Set.count <> List.length a
let private splitAtElement el a = 
    let idx = List.findIndex ((=) el) a 
    (List.take idx a, List.skip (idx + 1) a)

let treeFromTraversals inorder preorder : Result<Tree, string> =
    let rec loop io po : Tree = 
        match (io, po) with 
            | ([], []) -> Empty 
            | ([a], [b]) when a = b -> Node(a, Empty, Empty)
            | (io, root :: po_rest) -> 
                let (lft_io, rgt_io) = io |> splitAtElement root
                let lft_sz = lft_io |> List.length 
                // split preorder rest in subtrees at lft_sz
                let (lft_po, rgt_po) = po_rest |> List.splitAt lft_sz 
                // recurse
                Node(root, loop lft_io lft_po, loop rgt_io rgt_po)
            | _ -> failwith "unreachable"

    if   notSameLength inorder preorder then Error "traversals must have the same length"
    elif notSameElements inorder preorder then Error "traversals must have the same elements"
    elif notAllDistinct inorder || notAllDistinct preorder then Error "traversals must contain unique items"
    else Ok (loop inorder preorder)
