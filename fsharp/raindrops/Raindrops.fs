module Raindrops

let is_divisible (number: int, divisor: int): string = 
    if number % divisor = 0 then
       match divisor with
       | 3 -> "Pling"
       | 5 -> "Plang"
       | 7 -> "Plong"
       | _ -> ""
    else ""

let convert (number: int): string = 
    let divisors = [3..2..7]
    let result = List.fold (fun acc div -> acc + is_divisible (number, div)) "" divisors         
    if result.Length > 0 then 
        result
    else number.ToString()

