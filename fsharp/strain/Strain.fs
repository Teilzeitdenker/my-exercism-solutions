module Seq

let keep pred xs = 
    Seq.filter pred xs

let discard pred xs = 
    Seq.filter (fun x -> not (pred x)) xs