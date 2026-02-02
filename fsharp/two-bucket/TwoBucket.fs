module TwoBucket

type Bucket = One | Two

type BucketResult = {
    Moves: int 
    GoalBucket: Bucket 
    OtherBucket: int }

type State = int * int

type Move = 
    | FillOne
    | FillTwo
    | EmptyOne
    | EmptyTwo
    | PourRight
    | PourLeft

let allMoves = [FillOne; FillTwo; EmptyOne; EmptyTwo; PourRight; PourLeft]

let measure sizeOne sizeTwo (goal: int) (startBucket: Bucket) =
    let initialState = if startBucket = Bucket.One then (sizeOne, 0) else (0, sizeTwo)
    let forbiddenState = if startBucket = Bucket.One then (0, sizeTwo) else (sizeOne, 0)
    let applyMove ((a, b): State) (move: Move)  = 
        match move with
        | FillOne   -> (sizeOne, b) 
        | FillTwo   -> (a, sizeTwo)
        | EmptyOne  -> (0, b)
        | EmptyTwo  -> (a, 0)
        | PourLeft  -> 
            let toPour = min (sizeOne - a) b 
            (a + toPour, b - toPour)
        | PourRight -> 
            let toPour = min (sizeTwo - b) a 
            (a - toPour, b + toPour)
    let getNextStates (states, explored) = 
        match
            states 
            |> List.collect (fun st -> allMoves |> List.map (applyMove st) |> List.except explored) 
            |> List.distinct
        with 
        | []        -> None 
        | nxtStates -> Some (states, (nxtStates, explored |> List.append nxtStates))
    let allReachableStates = List.unfold getNextStates ([initialState], [forbiddenState])
    let tryBucketResult (idx, ls) =  
        match (List.tryFind (fst >> (=) goal) ls, List.tryFind (snd >> (=) goal) ls) with 
        | (Some (_, other), _) -> Some { Moves = idx + 1; GoalBucket = Bucket.One; OtherBucket = other }
        | (_, Some (other, _)) -> Some { Moves = idx + 1; GoalBucket = Bucket.Two; OtherBucket = other }
        | _                    -> None
    allReachableStates
    |> List.indexed 
    |> List.choose tryBucketResult
    |> List.head 