module Rectangles

let rectangles lines = 
    let get_corner_index_pairs line = 
        (Seq.allPairs line line)
        |> Seq.filter(fun (a, b) -> a < b)

    let corner_index_pairs = 
        lines 
        |> Seq.map(fun line -> 
            line 
            |> Seq.indexed
            |> Seq.filter(fun (_, c) -> c = '+') 
            |> Seq.map(fun (i, _) -> i) 
            |> get_corner_index_pairs)
        |> Seq.indexed

    let get_row2_candidates pair from_row =
        corner_index_pairs
        |> Seq.skip (from_row + 1)
        |> Seq.filter(fun (_, pairs) -> pairs |> Seq.contains pair)
        |> Seq.map(fun (row2, _) -> row2)

    let horizontal = ['+'; '-']
    let vertical = ['+'; '|']

    let check_rectangle row1 row2 (col1, col2) =
        lines                  |> Seq.item row1 |> Seq.skip (col1 + 1) |> Seq.take (col2 - col1 - 1) |> Seq.forall (fun c -> horizontal |> Seq.contains c) &&
        lines                  |> Seq.item row2 |> Seq.skip (col1 + 1) |> Seq.take (col2 - col1 - 1) |> Seq.forall (fun c -> horizontal |> Seq.contains c) &&
        lines |> Seq.transpose |> Seq.item col1 |> Seq.skip (row1 + 1) |> Seq.take (row2 - row1 - 1) |> Seq.forall (fun c -> vertical   |> Seq.contains c) &&
        lines |> Seq.transpose |> Seq.item col2 |> Seq.skip (row1 + 1) |> Seq.take (row2 - row1 - 1) |> Seq.forall (fun c -> vertical   |> Seq.contains c)

    corner_index_pairs 
    |> Seq.map(fun (row1, pairs) -> 
        pairs 
        |> Seq.map(fun pair ->
            (get_row2_candidates pair row1)
            |> Seq.filter(fun row2 -> check_rectangle row1 row2 pair)
            |> Seq.length
            )
        |> Seq.sum
        )
    |> Seq.sum