module ArmstrongNumbers

open System

let digitsReversed (number: int): int seq = 
    let mutable temp = number
    seq {while temp <> 0 do
            yield temp % 10
            temp <- temp / 10 }

let isArmstrongNumber (number: int): bool = 
    let exponent = Math.Floor(Math.Log10(number |> double) + 1.)
    digitsReversed number |> Seq.sumBy (fun elem -> Math.Pow(elem |> double, exponent) |> int) = number

