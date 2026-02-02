module RationalNumbers

open System

[<Struct>]
type Fraction = { Num: int; Den: int }

let rec gcd a b = if b = 0 then a else gcd b (a % b)

let create numerator denominator = 
    let g = gcd numerator denominator
    if (g > 0 && denominator > 0) || (g < 0 && denominator < 0) then
        { Num = numerator / g; Den = denominator / g}
    else
        { Num = - numerator / g; Den = - denominator / g }

let add (r1: Fraction) (r2: Fraction) = create (r1.Num * r2.Den + r1.Den * r2.Num) (r1.Den * r2.Den)

let sub (r1: Fraction) (r2: Fraction) = create (r1.Num * r2.Den - r1.Den * r2.Num) (r1.Den * r2.Den)

let mul (r1: Fraction) (r2: Fraction) = create (r1.Num * r2.Num) (r1.Den * r2.Den)

let inv (r: Fraction) = create (r.Den) (r.Num)

let div (r1: Fraction) (r2: Fraction) = mul r1 (inv r2)

let abs (r: Fraction) = create (abs (r.Num)) (abs (r.Den))

let exprational n (r: Fraction) = create (int <| Math.Pow(double r.Num, double n)) (int <| Math.Pow(double r.Den, double n))

let expreal (r: Fraction) n = Math.Pow(double n, (double (r.Num) / double (r.Den)))

let reduce (r: Fraction) = create r.Num r.Den