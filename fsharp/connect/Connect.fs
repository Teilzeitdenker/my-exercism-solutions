module Connect

type Player = White | Black 

type private Board = string list 

let private offsets = [| (-1, -1); (1, -1); (0, -2); (0, 2); (-1, 1); (1, 1) |]

let winner (board: Board) : Player option =
    let height = board |> List.length
    let width = ((board[0] |> String.length) - (height - 1)) / 2 + 1
    let onBoard r c =  r >= 0 && r <= height - 1 && c >= r && c <= r + 2 * (width - 1)
    let rec isConnected r c stone visited toEndEdge =
        if visited |> List.contains (r, c) then false
        else 
            if not (onBoard r c) || board[r][c] <> stone then false
            elif toEndEdge (r, c) then true 
            else offsets |> Seq.exists (fun (dr, dc) ->
                isConnected (r + dr) (c + dc) stone ((r, c) :: visited) toEndEdge)
    let toRightEdge (r, c) = c = r + 2 * (width - 1)
    let toBottomEdge (r, _ ) = r = height - 1 
    if  [0..(height - 1)] |> Seq.exists (fun r -> isConnected r r 'X' [] toRightEdge) then
        Some Black
    elif [0..(width - 1)] |> Seq.exists (fun c -> isConnected 0 (2 * c) 'O' [] toBottomEdge) then 
        Some White 
    else None