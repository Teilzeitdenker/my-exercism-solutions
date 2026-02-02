module LinkedList

type Node = {
    Data : int 
    mutable Next : Node option 
    mutable Prev : Node option 
}

type LinkedList = {
    mutable Head : Node option 
    mutable Tail : Node option 
}

let mkLinkedList () = { Head = None; Tail = None }

let pop linkedList = 
    match linkedList.Tail with
        | None -> failwith (invalidOp "empty list")
        | Some tailNode ->
            let data = tailNode.Data
            match tailNode.Prev with 
                | None -> 
                    linkedList.Tail <- None
                    linkedList.Head <- None 
                | Some newTail ->
                    newTail.Next <- None 
                    linkedList.Tail <- Some newTail
            data

let shift linkedList =
    match linkedList.Head with
        | None -> failwith (invalidOp "empty list")
        | Some headNode ->
            let data = headNode.Data
            match headNode.Next with 
                | None -> 
                    linkedList.Head <- None
                    linkedList.Tail <- None 
                | Some newHead ->
                    newHead.Prev <- None 
                    linkedList.Head <- Some newHead
            data

let push newValue linkedList = 
    match linkedList.Tail with 
        | None ->
            let newNode = { Data = newValue; Next = None; Prev = None }
            linkedList.Head <- Some newNode 
            linkedList.Tail <- Some newNode
        | Some oldTail -> 
            let newNode = { Data = newValue; Next = None; Prev = Some oldTail }
            oldTail.Next <- Some newNode
            linkedList.Tail <- Some newNode 

let unshift newValue linkedList =
    match linkedList.Head with 
        | None ->
            let newNode = { Data = newValue; Next = None; Prev = None }
            linkedList.Head <- Some newNode 
            linkedList.Tail <- Some newNode
        | Some oldHead -> 
            let newNode = { Data = newValue; Next = Some oldHead; Prev = None }
            oldHead.Prev <- Some newNode
            linkedList.Head <- Some newNode 
