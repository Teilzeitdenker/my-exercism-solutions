module SgfParsing

open FParsec

type Node = Node of Map<string, string list> * Node list

type private UserState = unit
type private Parser<'t> = Parser<'t, UserState>

let private str = pstring
let private propertyValue = (many1Chars (noneOf "]")) |> between (str "[") (str "]")
let private property = many1Chars asciiUpper .>>. (many1 propertyValue)
let private data = (many property) |>> Map.ofList
let private startOfChild = opt (str "(") >>. str ";"
let private endOfChild = opt (str ")")
let private startOfInput = str "(;"
let private endOfInput = endOfChild .>> eof
let private child = 
    startOfChild >>. data .>> endOfChild 
    |>> (fun d -> Node(d, []))
let private tree = 
    startOfInput >>. data .>>. (many child) .>> endOfInput 
    |>> (fun (d, ch) -> Node(d, ch))

let parse (input: string) : Node option =
    match run tree input with
    | Success(result, _, _)   -> Some result
    | Failure(_, _, _) -> None