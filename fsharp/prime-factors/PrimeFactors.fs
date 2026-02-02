module PrimeFactors

open System

let factors (number: int64) : int32 list = 
    if number <= 1L then []
    else 
        let mutable num = number
        let mutable primes = []
        while num % 2L = 0L do
            num <- num / 2L
            primes <- 2 :: primes
        for n in [3..2..(Math.Sqrt(num |> float) |> int)] do
            let k = int64 n
            while num % k = 0L do
                num <- num / k
                primes <- primes @ [n]
        if num > 1L then primes @ [num |> int]
        else
            primes
            
        