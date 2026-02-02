module RestApi

open System.Text.Json
open System.Collections.Generic
open System.Text.Json.Serialization

// following workaround is only necessary with floats to avoid getting 
// rounded values after serialization, no problem when using decimal instead

// open System.Globalization

//type FloatConverter() = 
//    inherit JsonConverter<float>()
//    override _.Read(_, _, _) = failwith "not needed"
//    override _.Write(writer, value, _) = 
//        writer.WriteRawValue(value.ToString("0.0#", CultureInfo.InvariantCulture))

//let serializerOptions : JsonSerializerOptions = 
//    let options = new JsonSerializerOptions()
//    options.Converters.Add(new FloatConverter())
//    options

let emptyDict = new SortedDictionary<string, decimal>()

let serialize o = JsonSerializer.Serialize o // use serializerOptions default value

type UserObject = {
    [<JsonPropertyName("name")>]
    Name : string
    [<JsonPropertyName("owes")>]
    Owes : SortedDictionary<string, decimal> 
    [<JsonPropertyName("owed_by")>]
    Owed_by : SortedDictionary<string, decimal>
    [<JsonPropertyName("balance")>]
    Balance : decimal
}

type User = { 
    [<JsonPropertyName("user")>]
    User : string }

type UserList = { 
    [<JsonPropertyName("users")>]
    Users : string list }

type Database = { 
    [<JsonPropertyName("users")>]
    Users : UserObject list }

type IOU = {
    [<JsonPropertyName("lender")>]
    Lender : string
    [<JsonPropertyName("borrower")>]
    Borrower : string 
    [<JsonPropertyName("amount")>]
    Amount : decimal
}

let toUserList (db : Database) : UserList = 
    { Users = db.Users |> List.map (fun user -> user.Name) }

// supports several successive requests by mutability (not required by the tests though)
type RestApi(database : string) =
    let mutable db = JsonSerializer.Deserialize<Database>(database)
    member this.Get(url: string) =
        match url with 
        | "/users" -> serialize (toUserList db)
        | _        -> failwith "invalid url sent with get request"
    member this.Get(url: string, payload: string) =
        match url with 
        | "/users" -> 
            let wanted = JsonSerializer.Deserialize<UserList>(payload).Users
            let res = { Users = db.Users |> List.filter (fun u -> wanted |> List.contains u.Name) }
            serialize res
        | _        -> failwith "invalid url sent with get request"
    member this.Post(url: string, payload: string)  =
        match url with 
        | "/add" -> 
            let user = JsonSerializer.Deserialize<User>(payload).User
            let userObject = { Name = user; Owes = emptyDict; Owed_by = emptyDict; Balance = 0.0m }
            db <- { Users = (userObject :: db.Users) |> List.sortBy (fun u -> u.Name) }
            serialize userObject
        | "/iou" ->
            let iou = JsonSerializer.Deserialize<IOU>(payload)
            let lenderObject   = db.Users |> List.find (fun user -> user.Name = iou.Lender)
            let borrowerObject = db.Users |> List.find (fun user -> user.Name = iou.Borrower)
            if lenderObject.Owes.ContainsKey(iou.Borrower) then 
                match lenderObject.Owes[iou.Borrower] with 
                | debt when debt = iou.Amount -> 
                    lenderObject.Owes.Remove(iou.Borrower) |> ignore
                    borrowerObject.Owed_by.Remove(iou.Lender) |> ignore
                | debt when debt > iou.Amount -> 
                    lenderObject.Owes[iou.Borrower] <- debt - iou.Amount
                    borrowerObject.Owed_by[iou.Lender] <- debt - iou.Amount
                | debt                        -> 
                    let newEntry = iou.Amount - debt 
                    lenderObject.Owes.Remove(iou.Borrower) |> ignore
                    lenderObject.Owed_by.Add(iou.Borrower, newEntry)
                    borrowerObject.Owed_by.Remove(iou.Lender) |> ignore
                    borrowerObject.Owes.Add(iou.Lender, newEntry)
            else 
                lenderObject.Owed_by.Add(iou.Borrower, iou.Amount)
                borrowerObject.Owes.Add(iou.Lender, iou.Amount)  
            let newlenderObject = { lenderObject with Balance = lenderObject.Balance + iou.Amount }
            let newBorrowerObject = { borrowerObject with Balance = borrowerObject.Balance - iou.Amount }
            let otherUsers = db.Users |> List.filter (fun user -> user.Name <> iou.Lender && user.Name <> iou.Borrower)
            db <- { Users = (newlenderObject :: newBorrowerObject :: otherUsers) |> List.sortBy (fun u -> u.Name) }
            let res = { Users = [newlenderObject; newBorrowerObject] |> List.sortBy (fun u -> u.Name) }
            serialize res
        | _      -> failwith "invalid url sent with post request"
