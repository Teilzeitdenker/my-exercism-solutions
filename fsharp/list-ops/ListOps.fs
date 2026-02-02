module ListOps

let rec foldl folder state list = List.fold folder state list 

let rec foldr folder state list = List.foldBack folder list state

let length = List.length

let reverse = List.rev

let map f = List.map f 

let filter f = List.filter f 

let append xs ys = xs @ ys

let concat = List.concat