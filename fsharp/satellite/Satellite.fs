module Satellite

type Tree =
    | Empty
    | Node of value: string * left: Tree * right: Tree

let private splitAtElement el a = (List.takeWhile ((<>) el) a, List.skipWhile ((<>) el) a |> List.tail)

let treeFromTraversals inorder preorder : Result<Tree, string> =
    let rec loop io po : Tree = 
        match po with 
            | [] -> Empty 
            | [a] -> Node(a, Empty, Empty)
            | root :: rest -> 
                let (lft_io, rgt_io) = io |> splitAtElement root
                let (lft_po, rgt_po) = rest |> List.splitAt (lft_io |> List.length)
                Node(root, loop lft_io lft_po, loop rgt_io rgt_po)

    if   List.length inorder <> List.length preorder then Error "traversals must have the same length"
    elif Set.ofList inorder <> Set.ofList preorder then Error "traversals must have the same elements"
    elif inorder <> List.distinct inorder || preorder <> List.distinct preorder then Error "traversals must contain unique items"
    else Ok (loop inorder preorder)
