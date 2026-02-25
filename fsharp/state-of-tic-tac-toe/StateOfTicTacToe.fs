module StateOfTicTacToe

type EndGameState = Win | Draw | Ongoing
type GameError = ConsecutiveMovesBySamePlayer | WrongPlayerStarted | MoveMadeAfterGameWasDone

let private checkWin (board : 'a[,]) player =
    let winLines = 
        [| 
           yield! Array.init 3 (fun i -> board[i, *]) // rows
           yield! Array.init 3 (fun i -> board[*, i]) // columns
           yield  Array.init 3 (fun i -> board[i, i]) // main diagonal
           yield  Array.init 3 (fun i -> board[i, 2-i]) // anti-diagonal
        |]
    winLines |> Array.exists (fun line -> line |> Array.forall ((=) player))

let gameState board : Result<EndGameState, GameError> =
    let xCount = board |> Seq.cast<char> |> Seq.filter ((=) 'X') |> Seq.length
    let oCount = board |> Seq.cast<char> |> Seq.filter ((=) 'O') |> Seq.length
    let xWon = checkWin board 'X'
    let oWon = checkWin board 'O'

    if   xCount < oCount     then Error WrongPlayerStarted
    elif xCount > oCount + 1 then Error ConsecutiveMovesBySamePlayer
    elif xWon   && oWon      then Error MoveMadeAfterGameWasDone
    elif xWon   || oWon      then Ok Win
    elif xCount + oCount = 9 then Ok Draw
    else Ok Ongoing
