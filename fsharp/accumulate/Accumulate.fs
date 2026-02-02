module Accumulate


// see https://blog.ploeh.dk/2015/12/22/tail-recurse/  under  "efficient tail-recursive map using a difference list"
// the accumulator is a function in this case
// one could also use the append-function @ directly on an accumulator list, but this wouldn't be performant
// or one takes the cons :: operator and reverses the list with List.rev (but then we would use a standard library function)
let accumulate (func: 'a -> 'b) (input: 'a list): 'b list = 
    let cons x xs = x :: xs 
    let rec mapImp f acc = function
        | [] -> acc []
        | h::t -> mapImp f (acc << (cons (f h))) t 
    mapImp func id input
