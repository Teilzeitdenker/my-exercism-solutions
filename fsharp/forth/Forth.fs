module Forth

let newCommands (commands: Map<string, list<int> -> option<list<int>>>) (commandTokens: list<string>) =
    match commandTokens with
    | key :: _ when (System.Int32.TryParse key) |> fst -> None
    | key :: rest ->

        let defaultFn = fun _ -> None

        let fn =
            (Some, rest)
            ||> List.fold (fun s t ->
                match System.Int32.TryParse t with
                | true, i -> s >> Option.map (fun xs -> i :: xs)
                | _, _ ->
                    let cmdFn =
                        (defaultFn, commands.TryFind t)
                        ||> Option.defaultValue

                    s >> Option.bind cmdFn)

        commands |> Map.add key fn |> Some
    | _ -> None

let rec eval (commands: Map<string, list<int> -> option<list<int>>>) (tokens: list<string>) (result: list<int>) =
    match tokens with
    | [] -> result |> List.rev |> Some
    | x :: xs ->
        match System.Int32.TryParse x with
        | true, i -> eval commands xs (i :: result)
        | _, _ ->
            match commands.TryFind x with
            | Some f -> result |> f |> Option.bind (eval commands xs)
            | _ ->
                match x with
                | ":" ->
                    let commandTokens = xs |> List.takeWhile (fun t -> t <> ";")

                    (commands, commandTokens)
                    ||> newCommands
                    |> Option.bind (fun commands' ->
                        let newTokens =
                            xs
                            |> List.skipWhile (fun t -> t <> ";")
                            |> List.skip 1

                        eval commands' newTokens result)
                | _ -> None

let unary (f: int -> option<list<int>>) (xs: list<int>) =
    match xs with
    | x :: xs -> (f x) |> Option.map (fun r -> r @ xs)
    | _ -> None

let binary (f: int -> int -> option<list<int>>) (xs: list<int>) =
    match xs with
    | b :: a :: xs -> (f a b) |> Option.map (fun r -> r @ xs)
    | _ -> None

let evaluate (xs: list<string>) : option<list<int>> =
    let tokens =
        xs
        |> String.concat " "
        |> fun s ->
            s.ToUpperInvariant().Split [| ' ' |]
            |> Array.toList

    let commands =
        Map [ ("+", binary (fun a b -> Some [ a + b ]))
              ("-", binary (fun a b -> Some [ a - b ]))
              ("*", binary (fun a b -> Some [ a * b ]))
              ("/", binary (fun a b -> if b = 0 then None else Some [ a / b ]))
              ("DUP", unary (fun x -> Some [ x; x ]))
              ("DROP", unary (fun _ -> Some []))
              ("SWAP", binary (fun a b -> Some [ a; b ]))
              ("OVER", binary (fun a b -> Some [ a; b; a ])) ]

    eval commands tokens []
