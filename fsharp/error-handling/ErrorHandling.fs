module ErrorHandling

open System

let handleErrorByThrowingException() = raise <| Exception()

let handleErrorByReturningOption (input: string) : int option = 
    match Int32.TryParse input with
    | true, i -> Some i 
    | false, _ -> None

let handleErrorByReturningResult (input: string) : Result<int, string> = 
    match Int32.TryParse input with
    | true, i -> Ok i 
    | false, _ -> Error "Could not convert input to integer"

let bind switchFunction twoTrackInput = 
    match twoTrackInput with
    | Ok s -> switchFunction s
    | Error f -> Error f

let cleanupDisposablesWhenThrowingException (resource:IDisposable) = 
    try raise <| Exception()
    finally resource.Dispose()