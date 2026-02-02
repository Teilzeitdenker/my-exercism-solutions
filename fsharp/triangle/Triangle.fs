module Triangle

let isTriangle a b c =
    (a > 0.) && (b > 0.) && (c > 0.) && (a + b > c) && (a + c > b) && (b + c > a)

let equilateral (triangle: double list) =
    let a = triangle.[0]
    let b = triangle.[1]
    let c = triangle.[2]
    (isTriangle a b c) && a = b && b = c

let isosceles (triangle: double list) =
    let a = triangle.[0]
    let b = triangle.[1]
    let c = triangle.[2]
    (isTriangle a b c) && (a = b || b = c || a = c)

let scalene (triangle: double list) = 
    let a = triangle.[0]
    let b = triangle.[1]
    let c = triangle.[2]
    (isTriangle a b c) && a <> b && b <> c && a <> c
