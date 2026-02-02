module KindergartenGarden

type Plant = 
    | Violets
    | Radishes
    | Clover
    | Grass

let charToPlant (c : char) : Plant =
    match c with
    | 'V' -> Plant.Violets
    | 'R' -> Plant.Radishes
    | 'C' -> Plant.Clover
    | 'G' -> Plant.Grass
    |  _  -> raise <| System.ArgumentOutOfRangeException()

let plants (diagram : string) (student : string) : Plant list = 
    let rows = diagram.Split([|'\n'|], System.StringSplitOptions.RemoveEmptyEntries)
    let idx = int student.[0] - 65
    [charToPlant rows.[0].[2*idx]; charToPlant rows.[0].[2*idx + 1]; charToPlant rows.[1].[2*idx]; charToPlant rows.[1].[2*idx + 1]]