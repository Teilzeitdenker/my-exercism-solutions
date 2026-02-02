module Bob

let isQuestion (s:string) : bool = 
    if s.Trim() = "" then
        false
    else 
        s.Trim() |> Seq.last = '?'

let isUpperCase s : bool = 
    if Seq.where System.Char.IsLetter s |> Seq.length = 0 then 
        false 
    else 
        String.map System.Char.ToUpper s = s


let response (input: string): string = 
    let yelled : bool = isUpperCase input
    let question : bool = isQuestion input
    if input.Trim() = "" then 
        "Fine. Be that way!"
    elif yelled && question then
        "Calm down, I know what I'm doing!"
    elif question then 
        "Sure."
    elif yelled then 
        "Whoa, chill out!"
    else 
        "Whatever."


