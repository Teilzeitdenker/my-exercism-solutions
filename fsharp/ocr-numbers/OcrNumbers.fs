module OcrNumbers

open System

let chunkStr size str =
    let rec loop (s:string) accum =
        let branch = size < s.Length
        match branch with
        | true  -> loop (s.[size..]) (s.[0..size-1]::accum)
        | false -> s::accum
    (loop str []) |> List.rev

let decodeDigit ls = 
    let digits = [
        [" _ "; "| |"; "|_|"; "   "]; 
        ["   "; "  |"; "  |"; "   "]; 
        [" _ "; " _|"; "|_ "; "   "];
        [" _ "; " _|"; " _|"; "   "]; 
        ["   "; "|_|"; "  |"; "   "];
        [" _ "; "|_ "; " _|"; "   "]; 
        [" _ "; "|_ "; "|_|"; "   "];
        [" _ "; "  |"; "  |"; "   "]; 
        [" _ "; "|_|"; "|_|"; "   "];
        [" _ "; "|_|"; " _|"; "   "]
    ]
    match digits |> List.tryFindIndex((=) ls) with
        | Some(d) -> d.ToString()
        | None    -> "?"
    

let convert (input: string list) = 
    if (input |> List.length) % 4 <> 0 || input |> List.exists (fun line -> line.Length % 3 <> 0) then
        None 
    else
        input 
        |> List.chunkBySize 4 
        |> List.map(fun row -> 
            row 
            |> List.map (chunkStr 3) 
            |> List.transpose 
            |> List.map decodeDigit
            |> List.fold (+) "" )
        |> String.concat "," 
        |> Some
           