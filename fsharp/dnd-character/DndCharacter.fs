module DndCharacter

open System

let r = Random(DateTime.Now.Millisecond)

let modifier x =
    int (Math.Floor ((double x - 10.) * 0.5))

let ability() = 
    seq {r.Next(1,7);r.Next(1,7);r.Next(1,7);r.Next(1,7)} |> Seq.sortDescending |> Seq.take 3 |> Seq.sum

type Character = 
    {
        Strength:int
        Dexterity:int
        Constitution:int
        Intelligence:int
        Wisdom:int
        Charisma:int
        Hitpoints:int
    }

let createCharacter() =
    let c = ability()
    {
        Strength = ability()
        Dexterity = ability()
        Constitution = c
        Intelligence = ability()
        Wisdom = ability()
        Charisma = ability()
        Hitpoints = 10 + modifier c
    }
