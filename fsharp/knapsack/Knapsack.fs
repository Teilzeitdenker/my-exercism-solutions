module Knapsack

type Item = { weight: int; value: int }

let maximumValue (items : Item list) maximumWeight =
    let rec loop items w v = 
        match items with 
            | _ when w = 0             -> v
            | []                       -> v 
            | h :: t when h.weight > w -> loop t w v  
            | h :: t                   -> max (loop t w v) (loop t (w - h.weight) (v + h.value))
    loop items maximumWeight 0