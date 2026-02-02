module RobotSimulator

type Direction = North | East | South | West
type Position = int * int
[<Struct>]
type Robot = { direction: Direction; position: Position }

let create (direction : Direction) (position : Position) : Robot = 
   { direction = direction; position = position }
   

let turnLeft (direction: Direction) = 
    match direction with
    | Direction.North -> Direction.West
    | Direction.East -> Direction.North
    | Direction.South -> Direction.East
    | Direction.West -> Direction.South

let turnRight (direction: Direction) = 
    match direction with
    | Direction.North -> Direction.East
    | Direction.East -> Direction.South
    | Direction.South -> Direction.West
    | Direction.West -> Direction.North

let signatureByDirection (direction: Direction) : (int * int) =
    match direction with
    | Direction.North -> (0, 1)
    | Direction.East -> (1, 0)
    | Direction.South -> (0, -1)
    | Direction.West -> (-1, 0)

let moveOne (letter : char) (robot : Robot) : Robot =
    let signature = signatureByDirection robot.direction 
    match letter with
    | 'A' -> { direction = robot.direction; position = ((fst signature) + (fst robot.position), (snd signature) + (snd robot.position))}
    | 'L' -> { direction = turnLeft robot.direction; position = robot.position }
    | 'R' -> { direction = turnRight robot.direction; position = robot.position }
    |  _  -> raise <| new System.ArgumentOutOfRangeException()

let rec move (instructions : string) (robot : Robot) : Robot = 
    instructions
    |> Seq.fold (fun acc c -> moveOne c acc) robot
    
