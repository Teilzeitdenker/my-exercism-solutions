module SgfParsing

open FParsec

type Tree = Node of Data: Map<string, string list> * Children: Tree list

type private UserState = unit
type private Parser<'t> = Parser<'t, UserState>

let private str = pstring
let private propertyValue = many1Chars (noneOf "]") |> between (str "[") (str "]")
let private property = many1Chars asciiUpper .>>. many1 propertyValue
let private data = many property |>> Map.ofList
let private startOfChild = opt (str "(") >>. str ";"
let private endOfChild = opt (str ")")
let private startOfTree = str "(;"
let private endOfTree = endOfChild .>> eof
let private child = startOfChild >>. data .>> endOfChild |>> (fun d -> Node(d, []))
let private tree = startOfTree >>. data .>>. many child .>> endOfTree |>> Node

let parse (input: string) : Tree option =
    match run tree input with 
    | Success(t, _, _) -> Some t
    | _                -> None