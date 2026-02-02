module BankAccount

type BankAccount() =
    let lockObj = obj()
    member val Closed = true
        with get, set
    member val Balance = 0.0m
        with get, set
    member this.Update(change: decimal) =
        lock lockObj (fun () -> 
            this.Balance <- this.Balance + change)

let mkBankAccount(): BankAccount = BankAccount() 

let openAccount (account: BankAccount): BankAccount = 
    account.Closed <- false
    account

let closeAccount (account: BankAccount): BankAccount = 
    account.Closed <- true 
    account

let getBalance (account: BankAccount): decimal option = 
    match account.Closed with
    | true -> None
    | false -> account.Balance |> Some

let updateBalance (change: decimal) (account: BankAccount) = 
    account.Update(change)
    account