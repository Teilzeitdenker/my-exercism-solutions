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
    // tailored to be used with List.unfold
    let getNextStates ((states, explored): State list * State list) : (State list * (State list * State list)) option = 
        let nxtStates =
            states |> List.collect (fun st -> 
                allMoves 
                |> List.map (applyMove st)
                |> List.filter (fun ex -> not (List.contains ex explored)) 
            ) |> List.distinct
        if nxtStates = [] then 
            None 
        else
            Some (states, (nxtStates, explored |> List.append nxtStates))
    // put the initial state into getNextStates and unfold
    let allReachableStates : State list list =
        let start = if startBucket = Bucket.One then (sizeOne, 0) else (0, sizeTwo)
        let forbidden = if startBucket = Bucket.One then (0, sizeTwo) else (sizeOne, 0)
        List.unfold getNextStates ([start], [forbidden])
    // tailored to be used with List.choose to extract a BucketResult from a (idx, list of states) tuple
    let tryBucketResult (idx, ls) : BucketResult option = 
        if List.exists (fst >> (=) goal) ls  then 
            let (_, other) = List.find (fst >> (=) goal) ls 
            Some { Moves = idx + 1; GoalBucket = Bucket.One; OtherBucket = other }
        else if List.exists (snd >> (=) goal) ls then
            let (other, _) = List.find (snd >> (=) goal) ls 
            Some { Moves = idx + 1; GoalBucket = Bucket.Two; OtherBucket = other }
        else 
            None
    // use index of reachable state list to get number of necessary moves
    allReachableStates
    |> List.indexed 
    |> List.choose tryBucketResult
    |> List.head // the test cases here guarantee an existing solution, otherwise use tryHead and distinguish cases