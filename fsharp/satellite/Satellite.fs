module Satellite

type Tree =
    | Empty
    | Node of value: string * left: Tree * right: Tree

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
                // split preorder rest at size of left subtree
                let (lft_po, rgt_po) = po_rest |> List.splitAt (lft_io |> List.length)
                // recurse
                Node(root, loop lft_io lft_po, loop rgt_io rgt_po)
            | _ -> failwith "unreachable"

    if   List.length inorder <> List.length preorder then Error "traversals must have the same length"
    elif Set.ofList inorder <> Set.ofList preorder then Error "traversals must have the same elements"
    elif inorder <> List.distinct inorder || preorder <> List.distinct preorder then Error "traversals must contain unique items"
    else Ok (loop inorder preorder)
