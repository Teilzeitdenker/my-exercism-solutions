module TracksOnTracksOnTracks

let newList: string list = []

let existingList: string list = ["F#"; "Clojure"; "Haskell"]

let addLanguage (language: string) (languages: string list): string list =
    language::languages

let countLanguages (languages: string list): int = List.length languages

let reverseList(languages: string list): string list = List.rev languages

let excitingList (languages: string list): bool = match languages.Length with
    | 0 -> false
    | 1 -> languages.Head = "F#"
    | _ -> (languages.Head = "F#") || ((languages.Tail.Head = "F#") && (languages.Length = 2 || languages.Length = 3))
