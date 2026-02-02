module PythagoreanTriplet

let tripletsWithSum (sum: int): (int * int * int) list =
    [1..(sum/3)] 
    |> List.choose (fun a -> 
        let bs = 
            [(a+1)..(sum-a)/2] 
            |> List.filter (fun b -> 
                 a*a + b*b = (sum - a - b)*(sum - a - b))
        if bs |> List.length > 0 then Some (a, List.head bs, sum - a - List.head bs)
        else None  )