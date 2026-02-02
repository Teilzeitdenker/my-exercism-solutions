module Bowling

let newGame() = []

let roll pins game = game @ [pins]

let score game = 
    let rec score' remaining frame acc =
        match remaining, frame with
        //invalid rolls -> short tracked
        | p1::_, _ when p1 < 0 || p1 > 10 -> None
        // all valid tenth frame cases
        | 10::10::p3::[], 10 when p3 <= 10                 -> Some (20 + p3 + acc)
        | 10::p2::p3::[], 10 when p2 + p3 <= 10            -> Some (10 + p2 + p3 + acc)
        | p1::p2::p3::[], 10 when p1 + p2 = 10 && p3 <= 10 -> Some (10 + p3 + acc)
        | p1::p2::[]    , 10 when p1 + p2 < 10             -> Some (p1 + p2 + acc)
        // other cases -> strike, spare and open frame
        | 10::p2::p3::ps, _                   -> score' (p2::p3::ps) (frame + 1) (10 + p2 + p3 + acc)
        | p1::p2::p3::ps, _ when p1 + p2 = 10 -> score' (p3::ps)     (frame + 1) (p1 + p2 + p3 + acc)
        | p1::p2::ps    , _ when p1 + p2 < 10 -> score' ps           (frame + 1) (p1 + p2 + acc)
        // all other possibilities are invalid
        | _ -> None
    score' game 1 0