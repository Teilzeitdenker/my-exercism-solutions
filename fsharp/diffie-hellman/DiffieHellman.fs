module DiffieHellman
open System
open System.Numerics

let rndGen = new Random()

let private modExp (bs: BigInteger) (exponent: BigInteger) (modulus: BigInteger) : BigInteger = 
    let mutable result = 1I
    let mutable bs = bs % modulus
    let mutable exponent = exponent
    while exponent > 0I do
        if exponent % 2I = 1I then
            result <- (result * bs) % modulus
        else 
            result <- result
        exponent <- exponent >>> 1
        bs <- (bs * bs) % modulus
    result

let privateKey (primeP: BigInteger) =
    rndGen.Next(2, primeP |> int) |> BigInteger

let publicKey primeP primeG privateKey = 
    modExp primeG privateKey primeP

let secret primeP publicKey privateKey = 
    modExp publicKey privateKey primeP
