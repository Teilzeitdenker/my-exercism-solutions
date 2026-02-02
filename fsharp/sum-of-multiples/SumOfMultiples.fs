module SumOfMultiples

let sum (numbers: int list) (upperBound: int): int = 
    let betterNumbers = numbers |> List.filter ((<) 0)
    if betterNumbers.Length = 0 then 
        0
    else
        betterNumbers 
        |> Seq.collect (fun n -> [n..n..(upperBound-1)]) 
        |> Seq.distinct 
        |> Seq.sum
        