module SimpleCipher

type SimpleCipher(key: string) =
    let shiftEncode shiftChar c = 
        let zero = 'a' |> int
        let shiftKey = (shiftChar |> int) - zero
        char <| (int c - zero + shiftKey) % 26 + int zero 
        
    let shiftDecode shiftChar c = 
        let zero = 'a' |> int
        let shiftKey = (shiftChar |> int) - zero
        char <| (int c - zero - (shiftKey - 26) ) % 26 + int zero 
    let modulo = key |> String.length

    new() = 
        let rand = new System.Random()
        let randKey = [0..99] |> Seq.map (fun _ -> rand.Next(26) + ('a' |> int) |> char) |> Seq.toArray |> System.String
        new SimpleCipher(randKey)

    member val Key = key
        with get
       
    member this.Encode(plaintext: string) = 
        plaintext
        |> Seq.mapi (fun i c -> shiftEncode (this.Key.[i % modulo]) c)
        |> Seq.toArray
        |> System.String

    member this.Decode(ciphertext: string) = 
        ciphertext
        |> Seq.mapi (fun i c -> shiftDecode (this.Key.[i % modulo]) c)
        |> Seq.toArray
        |> System.String
    