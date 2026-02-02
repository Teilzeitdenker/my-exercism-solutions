module Bandwagoner

type Coach = {Name: string; FormerPlayer: bool}

type Stats = {Wins: int; Losses: int}

type Team = {Name: string; Coach: Coach; Stats: Stats}

let createCoach (name: string) (formerPlayer: bool): Coach =
    let thisCoach: Coach = {Name = name; FormerPlayer = formerPlayer}
    thisCoach

let createStats(wins: int) (losses: int): Stats =
   let thisStats: Stats = {Wins = wins; Losses = losses}
   thisStats

let createTeam(name: string) (coach: Coach)(stats: Stats): Team =
  let thisTeam: Team = {Name = name; Coach = coach; Stats = stats}
  thisTeam

let replaceCoach(team: Team) (coach: Coach): Team =
   let newTeam = {team with Coach = coach}
   newTeam

let isSameTeam(homeTeam: Team) (awayTeam: Team): bool =
   homeTeam = awayTeam

let rootForTeam(team: Team): bool =
    match team.Name with 
    | "Chicago Bulls" -> true
    | _ -> match team.Coach with 
           | {Name = "Gregg Popovich"} -> true 
           | {FormerPlayer = true} -> true 
           | _ -> match team.Stats with 
                  | {Wins = w} when w >= 60 -> true
                  | {Wins = w; Losses = l} when l > w -> true
                  | _ -> false
