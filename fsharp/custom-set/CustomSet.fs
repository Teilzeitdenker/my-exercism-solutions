module CustomSet

type CustomSet (items: int seq) =
    member this.items = items |> Seq.distinct |> Seq.sort |> Seq.toList 

let empty = new CustomSet([])

let singleton value = new CustomSet([value])

let isEmpty (set : CustomSet) = List.isEmpty set.items

let size (set : CustomSet) = List.length set.items

let fromList list = new CustomSet(list)

let toList (set : CustomSet) = set.items

let contains value (set : CustomSet) = List.contains value set.items

let insert value (set : CustomSet) = new CustomSet(value :: set.items)

let union (left : CustomSet) (right : CustomSet) = new CustomSet(left.items @ right.items)

let intersection (left : CustomSet) (right : CustomSet) = new CustomSet(left.items |> List.filter (fun x -> List.contains x right.items))

let difference (left : CustomSet) (right : CustomSet) = new CustomSet(left.items |> List.filter (fun x -> not (List.contains x right.items)))

let isSubsetOf (left : CustomSet) (right : CustomSet) =  left.items |> Seq.forall (fun x -> contains x right)

let isDisjointFrom (left : CustomSet) (right : CustomSet) = left.items |> Seq.forall (fun x -> not (contains x right))

let isEqualTo (left : CustomSet) (right : CustomSet) = left.items = right.items