module ResistorColorDuo

let colors: string list = ["black"; "brown"; "red"; "orange"; "yellow"; "green"; "blue"; "violet"; "grey"; "white"]

let colorCode (color: string) : int = colors |> List.findIndex (fun c -> c = color)

let value colors =  match colors with
                    | [] | [_] -> failwith "At least two colors are required"
                    | fst :: snd :: _ -> (colorCode fst) * 10 + (colorCode snd)
