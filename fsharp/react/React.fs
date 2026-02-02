module React

[<AbstractClass>]
type Cell() =
    member val changed = new Event<int>()
    [<CLIEvent>]
    member this.Changed = this.changed.Publish
    abstract member Value : int with get, set

type InputCell(value: int) = 
    inherit Cell()
    let mutable number = value
    override this.Value 
        with get() = number
        and set(newValue: int) =
            if newValue <> number then 
                number <- newValue 
                this.changed.Trigger(number)

type ComputeCell(producers: Cell list, compute: int list -> int) as this =
    inherit Cell()
    let mutable number = 0
    do this.Recompute
    do this.InputProducers |> List.map (fun cell -> cell.Changed.Add (fun _ -> this.Recompute)) |> ignore
    override this.Value 
        with get() = number
        and set(newValue: int) = failwith "value cannot be set on a ComputeCell!"
    member this.Recompute = 
        let newValue = producers |> List.map (fun i -> i.Value) |> compute  
        if newValue <> number then 
            number <- newValue
            this.changed.Trigger(number)
    member this.InputProducers : InputCell list = 
        let isInputCell c : InputCell option = tryUnbox (box c)
        let getInputList c : InputCell list = 
            match isInputCell c with 
            | Some inputCell -> [inputCell]
            | None -> (unbox<ComputeCell> (box c)).InputProducers
        producers |> List.collect getInputList |> List.distinct

type Reactor() = 
    member _.createInputCell (value: int) = new InputCell(value)
    member _.createComputeCell (producers: Cell list) (compute: int list -> int) =
        new ComputeCell(producers, compute)