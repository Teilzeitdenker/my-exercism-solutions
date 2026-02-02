module Wordy

open FParsec
type private UserState = unit 
type private Parser<'t> = Parser<'t, UserState>

type private Operation = 
    | Add of int 
    | Sub of int 
    | Mul of int 
    | Div of int
    static member operationsChoice : Parser<_> seq = 
        seq { pstring "plus"; pstring "minus"; pstring "multiplied by"; pstring "divided by" } 
    static member tupleToOperation (sign, n) = 
        match sign with 
        | "plus"          -> Add n 
        | "minus"         -> Sub n 
        | "multiplied by" -> Mul n
        | "divided by"    -> Div n 
        | _               -> failwith "unreachable"
    static member operateOn a = function
        | Add n -> a + n 
        | Sub n -> a - n 
        | Mul n -> a * n 
        | Div n -> a / n

let private parser = 
    let startOfQuestion = skipString "What is" .>> spaces1
    let endOfQuestion = skipString "?" .>> eof
    let int_ws = pint32 .>> spaces 
    let parseSign = (choice Operation.operationsChoice) .>> spaces1
    let parseOperation = parseSign .>>. int_ws |>> Operation.tupleToOperation
    let firstNumAndAllOperations = int_ws .>>. (many parseOperation)
    firstNumAndAllOperations |> between startOfQuestion endOfQuestion 

let private calculate (start, operations) = 
    operations |> Seq.fold Operation.operateOn start  

let answer (question : string) = 
    match run parser question with 
    | Failure(_, _, _)   -> None 
    | Success(res, _, _) -> calculate res |> Some