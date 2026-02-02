module WordCount

open System.Text.RegularExpressions

let countWords phrase = 
    Regex.Matches(phrase, @"\w+('\w+)*")
    |> Seq.map (fun m -> m.Value)
    |> Seq.countBy (fun w -> w.ToLowerInvariant())
    |> Map.ofSeq
        