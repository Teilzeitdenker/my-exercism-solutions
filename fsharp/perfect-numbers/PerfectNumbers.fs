//module PerfectNumbers

//type Classification = Perfect | Abundant | Deficient 

//let classify n : Classification option = 
//    if n <= 0 then None
//    else match seq [1..(n / 2)] |> Seq.filter (fun x -> n % x = 0) |> Seq.sum with
//          | r when r = n -> Perfect |> Some 
//          | r when r > n -> Abundant |> Some 
//          | _ -> Deficient |> Some

module PerfectNumbers
type Classification = Perfect | Abundant | Deficient 

let aliquot n = 
    [1 .. n/2]
    |> Seq.filter (fun i -> n % i = 0)
    |> Seq.sum

let classify n : Classification option = 
    if n <= 0 then None else
    match aliquot n  with
    | x when x = n -> Some Perfect
    | x when x > n -> Some Abundant
    | _ -> Some Deficient 

