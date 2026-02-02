module FlowerField

let annotate (input : string list) = 
    let flowers = 
        [ for r, line in List.indexed input do
          for c, ch in Seq.indexed line do
          if ch = '*' then yield (r, c) ] |> Set.ofList
    let neighbors (r, c) =
        [ for dr in -1 .. 1 do
          for dc in -1 .. 1 do
          if not (dr = 0 && dc = 0) then
              yield (r + dr, c + dc) ]
    input |> List.mapi (fun r line ->
        line |> Seq.mapi (fun c ch ->
            if ch = '*' then '*'
            else
                let count =
                    neighbors (r, c)
                    |> List.filter (fun pos -> flowers |> Set.contains pos)
                    |> List.length
                if count = 0 then ' ' else char (count + int '0')
        ) |> Seq.toArray |> System.String
    )