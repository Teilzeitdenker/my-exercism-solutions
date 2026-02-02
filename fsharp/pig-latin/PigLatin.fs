module PigLatin

open System

let rec translate (input: String) = 
    if input.Contains ' ' then
        String.Join(' ', input.Split(' ') |> Seq.map translate)
    else 
        let vowels = [|'a'; 'e'; 'i'; 'o'; 'u'|]
        let fst_vowel = input.IndexOfAny vowels
        if fst_vowel = 0 || input.StartsWith "xr" || input.StartsWith "yt" then
            input + "ay"
        else if input.Length = 2 && input[1] = 'y' then
            "y" + input[0].ToString() + "ay"
        else if input.IndexOf("yt") <> -1 then
            input.Substring(input.IndexOf("yt")) + input.Substring(0, input.IndexOf("yt")) + "ay"
        else
            let consclust = input.Substring(0, fst_vowel)
            if consclust.EndsWith('q') && input[fst_vowel] = 'u' then
                input.Substring(fst_vowel + 1) + consclust + "uay"
            else
                input.Substring(fst_vowel) + consclust + "ay"