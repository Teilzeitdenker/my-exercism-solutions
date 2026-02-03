module RelativeDistance

type FamilyTree = Map<string, string list>
type Graph = Map<string, Set<string>>

let buildGraph (familyTree : FamilyTree) : Graph =
    let addEdge source target =
        Map.change source (Option.defaultValue Set.empty >> Set.add target >> Some)
    
    familyTree
    |> Map.fold (fun graph parent children -> // use a mutable graph variable to many edges at once
        let mutable g = graph
        for child in children do
            g <- addEdge parent child g
            g <- addEdge child parent g
        for c1 in children do // sibling connections
            for c2 in children do
                if c1 <> c2 then 
                    g <- addEdge c1 c2 g
        g
    ) Map.empty

let degreeOfSeparation (familyTree : FamilyTree) (person1 : string) (person2 : string) : int option =
    let graph = buildGraph familyTree
    let rec bfs queue visited =
        match queue with
        | [] -> None // no connection found, queue empty
        | (current, dist) :: _ when current = person2 -> Some dist
        | (current, dist) :: rest ->
            let nextNodes =
                Map.tryFind current graph
                |> Option.defaultValue Set.empty
                |> Set.filter (fun n -> not (Set.contains n visited))
                |> Set.map (fun n -> (n, dist + 1))
                |> Set.toList
            bfs (rest @ nextNodes) (Set.add current visited)
    
    bfs [(person1, 0)] Set.empty