module SpiralMatrix

open System.Numerics

let spiralMatrix (size: int) = 
    let matrix = Array2D.zeroCreate size size
    // let len = matrix.GetLength 0
    let mutable leng = size
    let mutable position = Complex(0., -1.)
    let mutable direction = Complex(0., 1.)
    let mutable numSpiralPartsWithSize = 1
    let mutable counter = 1
    while leng > 0 do
        for i in [0..(leng - 1)] do
            position <- (position + direction)
            matrix.[int (position.Real), int (position.Imaginary)] <- counter
            counter <- (counter + 1)
        direction <- (direction * (- Complex.ImaginaryOne))
        if numSpiralPartsWithSize = 1 then
            numSpiralPartsWithSize <- 2
            leng <- (leng - 1)
        else 
            numSpiralPartsWithSize <- (numSpiralPartsWithSize - 1)
    [ 
        let height = matrix.GetLength 0
        for row in 0..height - 1  do
        yield matrix.[row,*] |> List.ofArray
    ]