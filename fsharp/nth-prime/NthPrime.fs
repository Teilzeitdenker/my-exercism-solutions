module NthPrime

open System
open System.Collections.Generic;


let primes = 
    let rec nextPrime n p primes =
        if primes |> Map.containsKey n then
            nextPrime (n + p) p primes
        else
            primes.Add(n, p)

    let rec prime n primes =
        seq {
            if primes |> Map.containsKey n then
                let p = primes.Item n
                yield! prime (n + 1) (nextPrime (n + p) p (primes.Remove n))
            else
                yield n
                yield! prime (n + 1) (primes.Add(n * n, n))
        }

    prime 2 Map.empty

let prime = function
    | b when b <= 0 -> None
    | n -> primes |> Seq.take(n) |> Seq.last |> Some