module Diamond

let make lastLetter = 
    let n = (int)lastLetter - (int)'A' // total whitespace for the half of every row
    let getRow letter =
        let x = (int)letter - (int)'A' // necessary whitespace measured from the middle of the row
        let leftPart = (List.replicate (n - x) ' ' ) @ [letter] @ (List.replicate x ' ')
        let rightPart = leftPart |> List.rev |> List.tail
        leftPart @ rightPart |> List.toArray |> System.String
    let upperPart = ['A'..lastLetter] |> List.map getRow
    let lowerPart = upperPart |> List.rev |> List.tail
    upperPart @ lowerPart |> String.concat "\n"