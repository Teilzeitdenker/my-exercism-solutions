module Pov

type Graph<'T> = { // tests nail down this naming, could introduce member functions instead, but well...
    value: 'T 
    children: Graph<'T> list }

type Path<'T> = Path of Value : 'T * LeftOfFocus : Graph<'T> list * RightOfFocus : Graph<'T> list 

type Zipper<'T> = Cursor of Focus : Graph<'T> * Paths : Path<'T> list

let mkGraph v ch = { value = v; children = ch }

let zipperFrom graph = Cursor(graph, []) 

let pathVal (Path(Value = v)) = v 

let traceFromZipper (Cursor(Focus = tr; Paths = paths )) =
    (List.map pathVal paths |> List.rev) @ [tr.value]

let down (Cursor(Focus = { value = v; children = chs }; Paths = paths)) = 
    match chs with // set cursor to first child (if existing) and prepend according path to paths
    | ch :: rs -> Cursor(ch, Path(v, [], rs) :: paths) |> Some 
    | _        -> None 

let right (Cursor(Focus = curr; Paths = paths)) = 
    match paths with // set cursor to next right child of the first path in paths list (if existing) and update this path
    | Path(v, lft, r :: rs) :: ps -> Cursor(r, Path(v, lft @ [curr], rs) :: ps) |> Some 
    | _                           -> None

let rec find v zipper = 
    let Cursor(Focus = f) = zipper
    if f.value = v then Some zipper 
    else 
        match down zipper |> Option.bind (find v) with 
        | Some x -> Some x 
        | None   -> right zipper |> Option.bind (find v) 

let rec reparent (Cursor(Focus = curr; Paths = paths)) = 
    match paths with 
    | []                      -> curr 
    | Path(v, lft, rgt) :: ps -> 
        let parentPerspective = reparent (Cursor(mkGraph v (lft @ rgt), ps))
        let newChildren = curr.children @ [parentPerspective]
        mkGraph curr.value newChildren

let fromPOV (v : 'T) (gr : Graph<'T>) : Graph<'T> option = 
    find v (zipperFrom gr) |> Option.map reparent

let tracePathBetween (v1 : 'T) (v2 : 'T) (gr : Graph<'T>) : 'T list option = 
    fromPOV v1 gr |> Option.bind (zipperFrom >> find v2 >> Option.map traceFromZipper)