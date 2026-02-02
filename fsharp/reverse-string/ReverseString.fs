module ReverseString

open System.Linq

let reverse (input: string): string = 
    System.String.Concat(input.Reverse().ToArray())