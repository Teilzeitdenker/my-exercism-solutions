module SimpleLinkedList

type LinkedList<'T> = 
    | Nil 
    | Data of ('T * LinkedList<'T>)

let nil = Nil

let create x n = Data (x, n)

let isNil x = x = Nil 

let next x = 
    match x with 
        | Nil -> failwith "list is empty"
        | Data (_, rest) -> rest

let datum x = 
    match x with 
        | Nil -> failwith "list is empty"
        | Data (t, _) -> t

let rec toList x = 
    match x with 
        | Nil -> []
        | Data (t, rest) -> t :: (toList rest)


let rec fromList xs = 
    match xs with 
        | [] -> Nil 
        | h :: rest -> Data (h, fromList rest)

let reverse x = 
    let rec loop x acc = 
        match x with 
            | Nil -> acc 
            | Data (t, rest) -> loop rest (Data (t, acc))
    loop x Nil