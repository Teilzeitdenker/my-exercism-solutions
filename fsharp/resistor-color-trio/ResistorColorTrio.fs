module ResistorColorTrio

let allColors: string list = ["black"; "brown"; "red"; "orange"; "yellow"; "green"; "blue"; "violet"; "grey"; "white"]

let exponents: int list = [1; 10; 100; 1000; 10000; 100000; 1000000; 10000000; 100000000; 1000000000]

let colorCode (color: string) : int = allColors |> List.findIndex ((=) color)

let value (colors : string list) =  ((colorCode colors[0]) * 10 + (colorCode colors[1])) * (exponents[colorCode colors[2]])

let label colors = 
    let value = value colors
    if value % 1000000 = 0 then sprintf "%d megaohms" (value / 1000000)
    elif value % 1000 = 0 then sprintf "%d kiloohms" (value / 1000)
    else sprintf "%d ohms" value
