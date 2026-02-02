module RunLengthEncoding

open System
open System.Text.RegularExpressions

let encode (input: String) = 
    Regex.Replace(input, @"(\D)\1+", fun m -> $"{m.Length.ToString()}{m.Value[0]}")

let decode (input: String) = 
    Regex.Replace(input, @"(\d+)(\D)", fun m -> new String(m.Groups[2].Value[0], Int32.Parse(m.Groups[1].Value)))