module SecretHandshake

// Create a list of functions. They all need to have the same signature
let addWink l = List.append l ["wink"]
let addDoubleBlink l = List.append l ["double blink"]
let addCloseYourEyes l = List.append l ["close your eyes"]
let addJump l = List.append l ["jump"]
let reverse l = List.rev l 

let actions = [addWink; addDoubleBlink; addCloseYourEyes; addJump; reverse]

let commands number = 
    let action = actions 
                    |> List.indexed
                    |> List.filter (fun (i, f) -> (number &&& (1 <<< i)) <> 0)
                    |> List.fold (fun acc (i, f) -> acc >> f) id
    [] |> action