module Knapsack

type Item = { weight: int; value: int }

let rec maximumValue items mx =
    match items with
    | [] -> 0
    | h::t when h.weight > mx -> maximumValue t mx
    | h::t -> max (maximumValue t mx) (h.value + (maximumValue t (mx - h.weight)))