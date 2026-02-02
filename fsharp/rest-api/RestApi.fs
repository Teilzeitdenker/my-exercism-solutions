module RestApi

open System.Text.Json
open System.Text.Json.Serialization
open System.Collections.Generic
open System.Globalization

let emptyDict = new SortedDictionary<string, float>()

type FloatConverter() = 
    inherit JsonConverter<float>()
    override _.Read(_, _, _) = failwith "not needed"
    override _.Write(writer, value, _) = 
        writer.WriteRawValue(value.ToString("0.0#", CultureInfo.InvariantCulture))

let serializerOptions : JsonSerializerOptions = 
    let options = new JsonSerializerOptions()
    options.Converters.Add(new FloatConverter())
    options

let serialize o = JsonSerializer.Serialize(o, serializerOptions)

type UserObject = {
    name : string
    owes : SortedDictionary<string, float> 
    owed_by : SortedDictionary<string, float>
    balance : float
}

type User = { user : string }

type UserList = { users : string list }

type Database = { users : UserObject list }

type IOU = {
    lender : string
    borrower : string 
    amount : float
}

let toUserList (db : Database) : UserList = 
    { users = db.users |> List.map (fun user -> user.name) }

type RestApi(database : string) =
    let mutable document = JsonSerializer.Deserialize<Database>(database)
    member this.Get(url: string) =
        match url with 
        | "/users" -> serialize (toUserList document)
        | _        -> failwith "invalid url sent with get request"
    member this.Get(url: string, payload: string) =
        match url with 
        | "/users" -> 
            let wanted = JsonSerializer.Deserialize<UserList>(payload).users
            let res = { users = document.users |> List.filter (fun u -> wanted |> List.contains u.name) }
            serialize res
        | _        -> failwith "invalid url sent with get request"
    member this.Post(url: string, payload: string)  =
        match url with 
        | "/add" -> 
            let user = JsonSerializer.Deserialize<User>(payload).user
            let userObject = { name = user; owes = emptyDict; owed_by = emptyDict; balance = 0.0 }
            document <- { users = (userObject :: document.users) |> List.sortBy (fun u -> u.name) }
            serialize userObject
        | "/iou" ->
            let iou = JsonSerializer.Deserialize<IOU>(payload)
            let lenderObject   = document.users |> List.find (fun user -> user.name = iou.lender)
            let borrowerObject = document.users |> List.find (fun user -> user.name = iou.borrower)
            if lenderObject.owes.ContainsKey(iou.borrower) then 
                match lenderObject.owes[iou.borrower] with 
                | debt when debt = iou.amount -> 
                    lenderObject.owes.Remove(iou.borrower) |> ignore
                    borrowerObject.owed_by.Remove(iou.lender) |> ignore
                | debt when debt > iou.amount -> 
                    lenderObject.owes[iou.borrower] <- debt - iou.amount
                    borrowerObject.owed_by[iou.lender] <- debt - iou.amount
                | debt                        -> 
                    let newEntry = iou.amount - debt 
                    lenderObject.owes.Remove(iou.borrower) |> ignore
                    lenderObject.owed_by.Add(iou.borrower, newEntry)
                    borrowerObject.owed_by.Remove(iou.lender) |> ignore
                    borrowerObject.owes.Add(iou.lender, newEntry)
            else 
                lenderObject.owed_by.Add(iou.borrower, iou.amount)
                borrowerObject.owes.Add(iou.lender, iou.amount)  
            let newlenderObject = { lenderObject with balance = lenderObject.balance + iou.amount }
            let newBorrowerObject = { borrowerObject with balance = borrowerObject.balance - iou.amount }
            let otherUsers = document.users |> List.filter (fun user -> user.name <> iou.lender && user.name <> iou.borrower)
            document <- { users = (newlenderObject :: newBorrowerObject :: otherUsers) |> List.sortBy (fun u -> u.name) }
            let res = { users = [newlenderObject; newBorrowerObject] |> List.sortBy (fun u -> u.name) }
            serialize res
        | _      -> failwith "invalid url sent with post request"
