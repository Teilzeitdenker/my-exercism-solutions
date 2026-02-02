module ComplexNumbers

open System

type Complex = { R: double; I: double }

let create real imaginary : Complex = { R = real; I = imaginary }

let mul (z1: Complex) (z2: Complex): Complex = 
    { R = z1.R * z2.R - z1.I * z2.I; I = z1.R * z2.I + z1.I * z2.R }

let add (z1: Complex) (z2: Complex): Complex = { R = z1.R + z2.R; I = z1.I + z2.I }

let sub (z1: Complex) (z2: Complex): Complex = { R = z1.R - z2.R; I = z1.I - z2.I }

let absSq (z: Complex) : double = Math.Pow(z.R, 2.) + Math.Pow(z.I, 2.)

let reciprocal (z: Complex): Complex = 
    { R = z.R / (absSq z); I = - z.I / (absSq z)}

let div (z1: Complex) (z2: Complex): Complex = 
    let z = reciprocal z2 |> mul z1
    { R = z.R; I = z.I }

let abs (z: Complex) : double = Math.Sqrt(absSq z)

let conjugate (z: Complex) : Complex = { R = z.R; I = - z.I }

let real (z: Complex) : double = z.R

let imaginary (z: Complex) : double = z.I

let exp (z: Complex) : Complex = mul { R = Math.Exp(z.R); I = 0. } { R = Math.Cos(z.I); I = Math.Sin(z.I) }