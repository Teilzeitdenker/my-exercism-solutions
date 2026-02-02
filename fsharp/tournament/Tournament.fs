module Tournament
    
let tally (input: string list): string list = 
    let header = $"Team                           | MP |  W |  D |  L |  P"
    let tl = 
        input 
        |> Seq.map (fun s -> s.Split([|';'|], System.StringSplitOptions.RemoveEmptyEntries))  // split each line
        |> Seq.filter (fun arr -> arr.Length >= 3)  // get rid of badly formatted results or empty rows
        |> Seq.map (fun arr -> {| teamA = arr.[0]; teamB = arr.[1]; result = arr.[2] |} ) // restructure the matches by named fields
        |> Seq.collect (fun m ->   // now split every match into two anonymous result records for each of the teams
            if m.result = "draw" then 
                seq [ {| team = m.teamA; result = 'D'; points = 1 |}; {| team = m.teamB; result = 'D'; points = 1 |} ]
            else // add some logic to correctly record the winning or losing team
                seq [ {| team = (if m.result = "win" then m.teamA else m.teamB); result = 'W'; points = 3 |}; 
                {| team = (if m.result = "win" then m.teamB else m.teamA); result = 'L'; points = 0 |} ] )
        |> Seq.groupBy (fun r -> r.team)  // group these results by team
        |> Seq.map (fun g ->  // get the necessary statistics from the results sequence of each team
            let results = snd g
            {|  Name = fst g
                MP = results |> Seq.length
                W = results |> Seq.filter (fun r -> r.result = 'W') |> Seq.length
                D = results |> Seq.filter (fun r -> r.result = 'D') |> Seq.length
                L = results |> Seq.filter (fun r -> r.result = 'L') |> Seq.length
                Points = results |> Seq.sumBy (fun r -> r.points)
                |} )
        |> Seq.sortBy (fun r -> r.Name) // sort first by name
        |> Seq.sortByDescending (fun r -> r.Points) // and then by points, this leaves the correct alphabetical order for teams with equal points
        |> Seq.map (fun r -> $"{r.Name,-30} | {r.MP,2} | {r.W,2} | {r.D,2} | {r.L,2} | {r.Points,2}")
        |> Seq.toList
    header :: tl // add the header to the front