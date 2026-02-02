module Bob

let isQuestion (s:string) : bool = 
    if s = "" then
        false
    else 
        s |> Seq.last = '?'

let containsLetters (s:string) =
    s |> Seq.exists System.Char.IsLetter

let isUpperCase s : bool = 
    if containsLetters s then 
        s.ToUpper() = s 
    else 
        false


let response (input: string): string = 
    let s = input.Trim()
    let yelled : bool = isUpperCase s
    let question : bool = isQuestion s
    if s = "" then 
        "Fine. Be that way!"
    elif yelled && question then
        "Calm down, I know what I'm doing!"
    elif question then 
        "Sure."
    elif yelled then 
        "Whoa, chill out!"
    else 
        "Whatever."


