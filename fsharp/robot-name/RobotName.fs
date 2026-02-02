module RobotName

open System
open System.Collections.Generic

type Robot = {
    runningNumber: int
    name: string }

let private rnd: Random = new Random()
let mutable private numRobots = 0
let private givenIDs = new HashSet<string>()

let getRandomChar() = 
    rnd.Next(65, 91) |> char

let getRandomNum() =
    rnd.Next(48, 58) |> char

let getRandomID() =
    let randomChars = [| getRandomChar(); getRandomChar(); getRandomNum(); getRandomNum(); getRandomNum() |]
    new string(randomChars)

let mkRobot() = 
    let mutable id = getRandomID()
    numRobots <- numRobots + 1
    while givenIDs.Contains(id) do
        id <- getRandomID()
    givenIDs.Add(id) |> ignore
    { runningNumber = numRobots; name = id }

let name (robot: Robot) = robot.name

let reset (robot: Robot) = 
    let mutable newid = getRandomID()
    while givenIDs.Contains(newid) do
        newid <- getRandomID()
    givenIDs.Add(newid) |> ignore
    {robot with name = newid}