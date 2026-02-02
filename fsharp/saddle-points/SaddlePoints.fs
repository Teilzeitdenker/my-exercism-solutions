module SaddlePoints

let saddlePoints (matrix: int list list) = 
    let numRows = matrix |> Seq.length
    if numRows = 0 then List.empty
    else
        let numColumns = matrix.[0] |> List.length
        let isSaddlePoint (ind: int*int) = 
            let num = matrix.[fst ind].[snd ind]
            [0..(numColumns - 1)] |> List.forall (fun j -> matrix.[fst ind].[j] <= num) 
            && [0..(numRows - 1)] |> List.forall (fun i -> matrix.[i].[snd ind] >= num)
        
        [0..(numRows - 1)] 
        |> List.collect (fun i -> 
            [0..(numColumns - 1)] 
            |> List.map (fun j -> (i,j)) 
            |> List.filter (fun ind -> isSaddlePoint(ind)))
        |> List.map (fun ind -> (fst ind + 1, snd ind + 1))
