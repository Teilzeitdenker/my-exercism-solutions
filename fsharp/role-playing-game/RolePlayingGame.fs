module RolePlayingGame

type Player = { 
    Name: string option
    Level: int
    Health: int
    Mana: int option
}

let introduce (player: Player): string = 
    match player.Name with
    | Some name -> name
    | None -> "Mighty Magician"

let revive (player: Player): Player option = 
    match player.Health with
    | health when health > 0 -> None
    | _ ->
        let newHealth = 100
        let newMana =
            match player.Level with
            | level when level >= 10 -> Some 100
            | _ -> player.Mana
        Some { player with Health = newHealth; Mana = newMana }

let castSpell (manaCost: int) (player: Player): Player * int =
    match player.Mana with
    | Some mana when mana >= manaCost ->
        let damage = manaCost * 2
        let newMana = mana - manaCost
        ({ player with Mana = Some newMana }, damage)
    | Some _ -> (player, 0)
    | None ->
        let newHealth = max 0 (player.Health - manaCost)
        ({ player with Health = newHealth }, 0)
