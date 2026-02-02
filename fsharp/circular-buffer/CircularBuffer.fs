module CircularBuffer

type RingBuffer(capacity: int) =
    let mutable _items = Array.init capacity (fun i -> 0) 
    member val Capacity = capacity
        with get 
    member val Reader = 0 
        with get, set 
    member val Writer = 0
        with get, set
    member this.Read() = 
        if this.Reader >= this.Writer then 
            raise <| System.Exception()
        else 
            let readIndex = this.Reader % this.Capacity
            this.Reader <- this.Reader + 1
            _items.[readIndex]
    member this.Write(value: int) =
        if (this.Writer - this.Reader = this.Capacity) then 
            raise <| System.Exception()
        else 
            let writeIndex = this.Writer % this.Capacity
            this.Writer <- this.Writer + 1
            _items.[writeIndex] <- value
            ()
    member this.Overwrite(value: int) =
        if (this.Writer - this.Reader < this.Capacity) then 
            this.Write(value)
        else
            let writeIndex = this.Reader % this.Capacity
            this.Writer <- this.Writer + 1
            this.Reader <- this.Reader + 1
            _items.[writeIndex] <- value
            ()

let mkCircularBuffer size = RingBuffer(size)

let clear (buffer: RingBuffer) = RingBuffer(buffer.Capacity)

let write value (buffer: RingBuffer): RingBuffer = 
    buffer.Write(value)
    buffer
        
let forceWrite value (buffer: RingBuffer) = 
    buffer.Overwrite(value)
    buffer

let read (buffer: RingBuffer): (int * RingBuffer) = 
    let value = buffer.Read()
    value, buffer