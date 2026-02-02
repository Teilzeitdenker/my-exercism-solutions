module RotationalCipher

let shiftBy shiftKey c = 
    if System.Char.IsLetter(c) then
        let zero = if System.Char.IsUpper(c) then 'A' else 'a'
        char <| (int c - int zero + shiftKey) % 26 + int zero 
    else 
        c

let rotate shiftKey text = 
    text 
    |> Seq.map (shiftBy shiftKey) 
    |> Seq.toArray 
    |> System.String
    