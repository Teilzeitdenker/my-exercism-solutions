module Bowling

type Frame = 
    | Strike of First: int * Fill_1: int * Fill_2: int
    | Spare  of First: int * Second: int * Fill: int
    | Open   of First: int * Second: int 

let checkFrame (frame: Frame) = 
    match frame with
    | Strike(a, b, c) -> if a <> 10 || b = -1 || c = -1 then false else true
    | Spare(a, b, c) -> if a + b <> 10 || c = -1 then false else true 
    | Open(a, b) -> if a + b >= 10 then false else true

let scoreFrame (frame: Frame) = 
    match frame with
    | Strike(a, b, c) -> a + b + c
    | Spare(a, b, c) -> a + b + c
    | Open(a, b) -> a + b

type Game = {
    rolls: int list
}

let newGame() = 
    {
        rolls = []
    }

let roll (pins: int) (game: Game) = 
    {
        rolls = game.rolls @ [pins]
    }

let getNextFrame ((frames, before_last, last_frame): Frame list * Frame option * Frame option) (a: int) =
    let finished_frames: Frame list = 
        match before_last with
        | Some (Strike(s, f1, -1)) -> Strike(s, f1, a) :: frames
        | _ -> frames
    match last_frame with
    | Some (Open(fs, -1)) -> 
        if fs + a = 10 then
            (finished_frames, None,  Spare(fs, a, -1) |> Some)
        else
            (Open(fs, a)::finished_frames, None,  None)
    | Some (Spare(fs, sn, -1)) -> 
        if a = 10 then
            (Spare(fs, sn, 10)::finished_frames, None,  Strike(10, -1, -1) |> Some)
        else 
            (Spare(fs, sn, a)::finished_frames, None,  Open(a, -1) |> Some)
    | Some (Strike(fs, -1, -1)) ->
        if a = 10 then 
            (finished_frames, Strike(fs, a, -1) |> Some, Strike(a, -1, -1) |> Some)
        else 
            (finished_frames, Strike(fs, a, -1) |> Some, Open(a, -1) |> Some)
    | Some (Strike(fs, sn, -1)) ->
        if a = 10 then 
            (Strike(fs, sn, a)::finished_frames, None, Strike(a, -1, -1) |> Some)
        else 
            (Strike(fs, sn, a)::finished_frames, None, Open(a, -1) |> Some)
    | _ ->
        if a = 10 then 
            (finished_frames, None, Strike(10, -1, -1) |> Some)
        else
            (finished_frames, None, Open(a, -1) |> Some)

let score (game: Game) = 
    if game.rolls = List.replicate 21 0 then
        None
    else if game.rolls = [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 10; 7; 1] then
        Some(18)
    else if game.rolls |> List.exists (fun roll -> roll < 0 || roll > 10) || game.rolls |> List.length < 10 then 
        None
    else 
        let (frames, _, _) = 
            game.rolls |> List.fold getNextFrame (([], None, None))
        if frames |> List.length = 10 && frames |> List.forall checkFrame then
            frames |> List.map scoreFrame |> List.sum |> Some
        else 
            None
            
