module SpaceAge

type Planet = 
    | Mercury = 1 
    | Venus = 2 
    | Earth = 3 
    | Mars = 4 
    | Jupiter = 5 
    | Saturn = 6 
    | Uranus = 7 
    | Neptune = 8

let age (planet: Planet) (seconds: int64): float = 
    let divisor1 = 31557600.0
    let divisor2 = 
        match (int planet) with 
        | 1 -> 0.2408467
        | 2 -> 0.61519726
        | 3 -> 1.0
        | 4 -> 1.8808158
        | 5 -> 11.862615
        | 6 -> 29.447498
        | 7 -> 84.016846
        | 8 -> 164.79132
        | _ -> 0.0
    (float seconds / divisor1) / divisor2