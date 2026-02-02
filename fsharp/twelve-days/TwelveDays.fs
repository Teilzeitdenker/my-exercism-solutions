module TwelveDays

open System

let numbers = ["a"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine"; "ten"; "eleven"; "twelve"]
let ordinals = ["first"; "second"; "third"; "fourth"; "fifth"; "sixth"; "seventh"; "eighth"; "ninth"; "tenth"; "eleventh"; "twelfth"]
let gifts = ["Partridge in a Pear Tree"; "Turtle Doves"; "French Hens"; "Calling Birds"; "Gold Rings"; "Geese-a-Laying"; "Swans-a-Swimming"; "Maids-a-Milking"; "Ladies Dancing"; "Lords-a-Leaping"; "Pipers Piping"; "Drummers Drumming"]



let verse number : string = 
    let ordinal = ordinals[number - 1]
    let first_gift = $"{numbers[0]} {gifts[0]}."
    let all_gifts = 
        if number = 1 then 
            first_gift 
        else 
            ([ for i in number-1..-1..1 -> $"{numbers[i]} {gifts[i]}" ] |> String.concat ", ") + ", and " + first_gift 
    $"On the {ordinal} day of Christmas my true love gave to me: {all_gifts}"

let recite start stop = [ for i in start..stop -> verse i ]