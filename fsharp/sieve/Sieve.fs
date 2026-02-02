module Sieve

let primes limit = 
    // collect the primes in a List and filter the range accordingly
    let rec sieve primes = function
        | []        -> primes |> List.rev
        | p :: rest -> sieve (p :: primes) (rest |> List.except [p*p..p..limit])
    sieve [] [2 .. limit] 