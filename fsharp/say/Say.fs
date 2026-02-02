module Say

let units = ["zero"; "one"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine"; "ten"; "eleven"; "twelve"; "thirteen"; "fourteen"; "fifteen"; "sixteen"; "seventeen"; "eighteen"; "nineteen"]
let tens = ["_"; "ten"; "twenty"; "thirty"; "forty"; "fifty"; "sixty"; "seventy"; "eighty"; "ninety"]

let say (number: int64) : string option = 
    
    let rec loop num fillChar = 
        match num with 
        | 0L                        -> ""
        | n when n < 20L            -> fillChar + units[(int)n]
        | n when n < 100L           -> fillChar + tens[(int)(n / 10L)]                        + loop (n % 10L) "-"
        | n when n < 1_000L         -> fillChar + loop (n / 100L) ""           + " hundred"   + loop (n % 100L) " "
        | n when n < 1_000_000L     -> fillChar + loop (n / 1_000L) ""         + " thousand"  + loop (n % 1_000L) " "
        | n when n < 1_000_000_000L -> fillChar + loop (n / 1_000_000L) ""     + " million"   + loop (n % 1_000_000L) " "
        | n                         -> fillChar + loop (n / 1_000_000_000L) "" + " billion"   + loop (n % 1_000_000_000L) " "
    
    match number with 
    | n when n < 0L                  -> None
    | n when n >= 1_000_000_000_000L -> None 
    | 0L                             -> Some "zero"
    | _                              -> loop number "" |> Some