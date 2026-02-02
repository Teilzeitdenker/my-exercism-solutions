module QueenAttack

let in_range (number: int) (range: list<int>) : bool =
   Seq.exists (fun x -> x = number) range

let create (position: int * int) : bool = 
    let admitted = [0..7]
    in_range (fst position) admitted && in_range (snd position) admitted

let canAttack (queen1: int * int) (queen2: int * int) = 
    if create queen1 && create queen2 then
        if fst queen1 = fst queen2 || snd queen1 = snd queen2 then
           true
        elif abs (fst queen1 - fst queen2) = abs (snd queen1 - snd queen2) then
           true
        else 
           false
    else 
        false