module Connect

type Player = White | Black 

let private offsets = [| (-1, -1); (1, -1); (0, -2); (0, 2); (-1, 1); (1, 1) |]

let winner board =
    let height = board |> List.length
    let width = ((board[0] |> String.length) - (height - 1)) / 2 + 1
    let onBoard r c =  r >= 0 && r <= height - 1 && c >= r && c <= r + 2 * (width - 1)

    let rec path r c stone visited toPlayerGoal =
        if visited |> List.contains (r, c) then false
        else 
            if not (onBoard r c) || board[r][c] <> stone then false
            elif toPlayerGoal (r, c) then true 
            else offsets |> Seq.exists (fun (dr, dc) ->
                path (r + dr) (c + dc) stone ((r, c)::visited) toPlayerGoal)
    
    let toBlackGoal (r, c) = c = r + 2 * (width - 1)
    let toWhiteGoal (r, _) = r = height - 1 
    
    if  [0..1..(height - 1)] |> Seq.exists (fun r -> path r r 'X' [] toBlackGoal) then
        Some Black
    elif [0..2..2*(width-1)] |> Seq.exists (fun c -> path 0 c 'O' [] toWhiteGoal) then 
        Some White 
    else None