module SimpleLinkedList

type LinkedList<'T> = 
    | Nil 
    | Data of ('T * LinkedList<'T>)

let nil = Nil

let create x n = Data (x, n)

let isNil x = x = Nil 

let next = function
    | Nil -> failwith "list is empty"
    | Data (_, rest) -> rest

let datum = function
    | Nil -> failwith "list is empty"
    | Data (t, _) -> t

let rec toList = function 
    | Nil -> []
    | Data (t, rest) -> t :: (toList rest)

let rec fromList ls = List.foldBack create ls Nil 
//function
//    | [] -> Nil 
//    | h :: rest -> Data (h, fromList rest)

let reverse x = 
    let rec loop acc = function
        | Nil -> acc 
        | Data (t, rest) -> loop (Data (t, acc)) rest
    loop Nil x