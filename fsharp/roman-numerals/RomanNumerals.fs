module RomanNumerals

let roman arabicNumeral = 
    if arabicNumeral >= 4000 then failwith "Number too big"
    let digits = arabicNumeral |> string |> Seq.map (int >> (+) (- int '0')) |> Seq.rev |> Seq.indexed
    let romanLetters = [("I", "V"); ("X", "L"); ("C", "D"); ("M", "?")]
    let getRightNumber (pos, digit) = 
        let (a, b) = romanLetters.[pos]
        match digit with 
        | 0 -> ""
        | 1 -> a 
        | 2 -> a + a 
        | 3 -> a + a + a 
        | 4 -> a + b 
        | 5 -> b 
        | 6 -> b + a 
        | 7 -> b + a + a 
        | 8 -> b + a + a + a 
        | 9 -> a + (fst (romanLetters.[pos + 1]))
        | _ -> failwith "not a digit"
    Seq.foldBack (fun el acc -> acc + getRightNumber el) digits ""