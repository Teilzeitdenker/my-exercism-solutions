module Minesweeper

let addMineCounts mc1 mc2 =
    if mc1 < 0 || mc2 < 0 then -1 else mc1 + mc2

let addLists l1 l2 =
    List.zip l1 l2 
    |> List.map (fun (mc1, mc2) -> addMineCounts mc1 mc2)

let addFlowers flower1 flower2 = 
    List.zip flower1 flower2 
    |> List.map (fun (l1, l2) -> addLists l1 l2)

let singleMineFlower col cols = 
    let edge   = List.replicate col 0 @ List.replicate 3 1 @ List.replicate (cols - col - 1) 0
    let middle = List.updateAt (col + 1) -1 edge
    [edge; middle; edge]

let getFlowersForRow row cols = 
    let defaultList = List.replicate 3 (List.replicate (cols + 2) 0)
    if row |> String.exists ((=) '*') then 
        row 
        |> Seq.indexed 
        |> Seq.filter (fun (_, ch) -> ch = '*')
        |> Seq.map (fun (col, _) -> singleMineFlower col cols)
        |> Seq.reduce addFlowers
    else defaultList

let getResultString resultRow = 
    let l = List.length resultRow
    resultRow |> List.skip 1 |> List.take (l - 2)
    |> List.map (fun num -> 
            match num with 
            | -1 -> "*"
            | 0  -> " "
            | n  -> n.ToString()   
        )
    |> String.concat ""

let cleanUp result = 
    let l = List.length result
    result |> List.skip 1 |> List.take (l - 2) |> List.map getResultString

let annotate input =  
    if input = [] || input = [""] then input
    else 
        let cols = input |> List.head |> String.length 
        input 
        |> List.map (fun row -> getFlowersForRow row cols)
        |> List.reduce (fun acc rowFlowers -> 
            let accLength = acc |> List.length
            (acc.[.. accLength - 3]) @ [
                addLists acc.[accLength - 2] rowFlowers.[0];
                addLists acc.[accLength - 1] rowFlowers.[1];
                rowFlowers.[2]
            ]
            )
        |> cleanUp