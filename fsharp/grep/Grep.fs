module Grep

open System
open System.Text.RegularExpressions
open System.IO

[<Flags>]
type GrepFlags = 
    | None =            0uy 
    | LineNums =        1uy 
    | OnlyFileNames =   2uy 
    | CaseInsensitive = 4uy
    | InvertMatch =     8uy
    | FullLineMatch =   16uy

let parseFlag (flag: string) = 
    match flag with 
    | "-n" -> GrepFlags.LineNums 
    | "-l" -> GrepFlags.OnlyFileNames 
    | "-i" -> GrepFlags.CaseInsensitive
    | "-v" -> GrepFlags.InvertMatch
    | "-x" -> GrepFlags.FullLineMatch
    |  _   -> GrepFlags.None 

let parseFlags (flagArguments: string list) = 
    flagArguments |> List.fold (fun acc flag -> acc ||| (parseFlag flag)) GrepFlags.None 
    
let format (use_files: bool) (use_nums: bool) (file: string) (n: int) (line: string) = 
    let file_format = if use_files then $"{file}:" else ""
    let num_format = if use_nums then $"{n}:" else ""
    $"{file_format}{num_format}{line}"

// no logical exclusive or operator for booleans in F# !!
let xor (a: bool) (b: bool) = 
    (a && not b) || (not a && b)

let grep (files: string list) (flagArguments: string list) (pattern: string) = 
    let flags = parseFlags flagArguments 
    let pattern_str = if flags.HasFlag GrepFlags.FullLineMatch then $"^{pattern}$" else pattern 
    let rgx_options = if flags.HasFlag GrepFlags.CaseInsensitive then RegexOptions.IgnoreCase else RegexOptions.None
    let rgx = new Regex(pattern_str, rgx_options)
    if flags.HasFlag GrepFlags.OnlyFileNames 
        then 
            files 
            |> List.filter (fun file -> 
                 File.ReadAllLines(file) 
                 |> Seq.exists(fun line -> 
                    xor (rgx.IsMatch(line)) (flags.HasFlag GrepFlags.InvertMatch)
                 )
            )
        else 
            files 
            |> List.collect (fun file -> 
                 File.ReadAllLines(file)
                 |> Seq.indexed
                 |> Seq.filter (fun (_, line) -> 
                    xor (rgx.IsMatch(line)) (flags.HasFlag GrepFlags.InvertMatch )
                 )
                 |> Seq.map (fun (n, line) -> 
                    format (files.Length > 1) (flags.HasFlag GrepFlags.LineNums) file (n+1) line
                 )
                 |> Seq.toList
            )