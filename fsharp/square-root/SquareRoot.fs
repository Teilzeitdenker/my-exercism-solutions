module SquareRoot

/// Calculate floor(log2(n)) by counting bit shifts
let private log2 n =
    let rec loop x exp =
        if x = 0 then exp - 1
        else loop (x >>> 1) (exp + 1)
    loop n 0

/// seed the number with n_0 = 2^(floor(log2(n))/2 + 1)
let private seed n =
    let logVal = log2 n
    let exp = (logVal / 2) + 1
    1 <<< exp  // 2^exp using bit shift

/// use Heron's method (also known as the Babylonian method) to find the integer square root
/// see https://en.wikipedia.org/wiki/Integer_square_root in the subsection "Using only integer division"
let private isqrt n seed =
    let rec loop curr =
        let nxt = (curr + n / curr) / 2
        if nxt >= curr then curr 
        else loop nxt 
    loop seed

let squareRoot n =
    isqrt n (seed n)
