module Knapsack

type Item = { weight: int; value: int }

let maximumValue (items : Item list) maximumWeight =
    let rec branch items w v = 
        if items |> List.isEmpty || w = 0 then v
        elif items.Head.weight > w then branch items.Tail w v 
        else max (branch items.Tail w v) (branch items.Tail (w - items.Head.weight) (v + items.Head.value))
    branch items maximumWeight 0