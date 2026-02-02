module ScrabbleScore

let toPoints = function
    | 'A'| 'E'| 'I'| 'O'| 'U'| 'L'| 'N'| 'R'| 'S'| 'T' -> 1 
    | 'D'| 'G'                                         -> 2
    | 'B'| 'C'| 'M'| 'P'                               -> 3
    | 'F'| 'H'| 'V'| 'W'| 'Y'                          -> 4
    | 'K'                                              -> 5
    | 'J'| 'X'                                         -> 8
    | _                                                -> 10

let score (word: string) = 
    word.ToUpper() |> Seq.map (toPoints) |> Seq.sum 