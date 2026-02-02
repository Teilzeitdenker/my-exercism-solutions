module NucleotideCount

let valid (strand: string): bool = 
    Set.isSubset (Seq.distinct strand |> Set.ofSeq) (set ['A';'T';'G';'C'])

let count (c: char) (strand:string): char * int = 
    c, strand |> Seq.filter (fun l -> l = c) |> Seq.length

let nucleotideCounts (strand: string): Option<Map<char, int>> = 
    if not (valid strand) then
        None
    else 
        seq { for c in seq {'A'; 'T'; 'G'; 'C'} do 
                count c strand } |> Map.ofSeq |> Some


        