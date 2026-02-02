module GameOfLife

let getValue r c (m: int[,]) = if r >= 0 && r < m.GetLength(0) && c >= 0 && c < m.GetLength(1) then m[r, c] else 0

let offsets = [| (-1, 0); (1, 0); (0, -1); (0, 1); (-1, -1); (-1, 1); (1, -1); (1, 1) |]

let getNumAliveNeighbors r c (m: int[,]) = offsets |> Array.fold (fun acc (x, y) -> acc + getValue (r + x) (c + y) m) 0

let tick (input: int[,]) = 
    let applyRules i j alive = 
        match (alive, getNumAliveNeighbors i j input) with 
        | (_, 3) -> 1 // stasis for alive cells, reproduction for dead cells,
        | (1, 2) -> 1 // stasis for alive cells
        | _      -> 0 // stasis for dead  cells, under- and overpopulation for alive cells
    input |> Array2D.mapi applyRules 