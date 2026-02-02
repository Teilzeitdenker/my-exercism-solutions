module LogLevels

let message (logLine: string): string = 
    let start: int = logLine.IndexOf ":" + 2
    logLine[start..].Trim()

let logLevel(logLine: string): string = 
    let last: int = logLine.IndexOf ":" - 2
    logLine[1..last].ToLower()

let reformat(logLine: string): string = 
    (message logLine) + " (" + (logLevel logLine) + ")"
