module Hangman

open System
open System.Reactive
open System.Reactive.Linq
open System.Reactive.Subjects

type Progress = Busy of int | Win | Lose 
type HangmanState = { progress: Progress; maskedWord: string }
type HangmanSubject(subject: BehaviorSubject<HangmanState>) = 
    member _.Add    = subject.Subscribe 
    member _.Value  = subject.Value  
    member _.AsObs  = subject.AsObservable 
    member _.OnNext = subject.OnNext
type Hangman(word: string) = 
    let initialMaskedWord = '_' |> Seq.replicate word.Length |> String.Concat
    let initialState = { progress = Busy 9; maskedWord = initialMaskedWord }
    let subject = new HangmanSubject(new BehaviorSubject<HangmanState>(initialState))
    member _.Start = subject.OnNext(initialState) 
    member _.StateObservable 
        with get() : IObservable<HangmanState> = subject.AsObs()
    member this.GuessObserver 
        with get() : IObserver<char> = Observer.Create<char>(fun c -> this.OnNextChar c)
    member this.OnNextChar (c : char) : unit = 
        let state = subject.Value
        let noSuccess = state.maskedWord |> Seq.contains c || word |> Seq.contains c |> not
        match state.progress with 
        | Busy 0                -> subject.OnNext({ state with progress = Lose })
        | Busy i when noSuccess -> subject.OnNext({ state with progress = Busy (i - 1) })
        | Busy i                -> 
            let newMasked = 
                Seq.zip state.maskedWord word
                |> Seq.map (fun (m, l) -> if l = c then c else m)
                |> String.Concat
            if newMasked = word then 
                subject.OnNext({ maskedWord = word; progress = Win })
            else 
                subject.OnNext({ state with maskedWord = newMasked })
        | _                     -> () 
        
let createGame word = new Hangman(word)
let startGame (game: Hangman) = game.Start
let statesObservable (game: Hangman) = game.StateObservable
let makeGuess x (game: Hangman) = game.GuessObserver.OnNext(x)