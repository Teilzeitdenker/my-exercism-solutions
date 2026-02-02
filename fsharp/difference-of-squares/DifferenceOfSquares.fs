module DifferenceOfSquares

let squareOfSum (n: int): int =
    let sumToN = (n*(n+1))/2
    sumToN*sumToN

let sumOfSquares (n: int): int = 
    (n*(n+1)*(2*n+1))/6

let differenceOfSquares (n: int): int = 
    squareOfSum n - sumOfSquares n