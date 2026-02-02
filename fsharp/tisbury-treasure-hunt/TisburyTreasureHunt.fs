module TisburyTreasureHunt

let getCoordinate (line: string * string): string =
    let _, c = line
    c

let convertCoordinate (coordinate: string): int * char = 
    (int coordinate.[0] - int '0', char coordinate.[1])

let compareRecords (azarasData: string * string) (ruisData: string * (int * char) * string) : bool = 
    let l, c, q = ruisData
    convertCoordinate (getCoordinate azarasData) = c

let createRecord (azarasData: string * string) (ruisData: string * (int * char) * string) : (string * string * string * string) =
    let t, c = azarasData
    let l, _, q = ruisData
    if compareRecords azarasData ruisData then (c, l, q, t)
    else ("", "", "", "")
