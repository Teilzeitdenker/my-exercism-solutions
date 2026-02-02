module Zipper

type BinTree = BinTree of Value:int * LeftTree:BinTree option * RightTree:BinTree option 

type BinPath = Top | Left of Value:int * Father:BinPath * RightSibling:BinTree option | Right of Value:int * LeftSibling:BinTree option * Father:BinPath 

type Zipper = Cursor of Tree:BinTree * Path:BinPath 

let tree value left right : BinTree = 
    BinTree(value, left, right) 

let fromTree t : Zipper = 
    Cursor(t, Top)

let value (Cursor(Tree = BinTree(Value = v))) =  v

let up (Cursor(Tree = t; Path = p)) =
    match p with 
    | Top -> None 
    | Left(v, father, right) -> Cursor(BinTree(v, Some t, right), father) |> Some
    | Right(v, left, father) -> Cursor(BinTree(v, left, Some t), father) |> Some 

let rec toTree cursor = 
    let Cursor(Tree = t; Path = p) = cursor
    match p with
    | Top -> t 
    | _   -> up cursor |> Option.get |> toTree

let left (Cursor(Tree = BinTree(Value = v; LeftTree = l; RightTree = r); Path = p)) = 
    match l with 
    | None -> None 
    | Some t -> Cursor(t, Left(v, p, r)) |> Some 

let right (Cursor(Tree = BinTree(Value = v; LeftTree = l; RightTree = r); Path = p)) = 
    match r with 
    | None -> None 
    | Some t -> Cursor(t, Right(v, l, p)) |> Some 
    
let setValue n (Cursor(Tree = BinTree(LeftTree = l; RightTree = r); Path = p)) = 
    Cursor(BinTree(n, l, r), p)

let setLeft t (Cursor(Tree = BinTree(Value = v; RightTree = r); Path = p)) =
    Cursor(BinTree(v, t, r), p)

let setRight t (Cursor(Tree = BinTree(Value = v; LeftTree = l); Path = p)) =
    Cursor(BinTree(v, l, t), p)
