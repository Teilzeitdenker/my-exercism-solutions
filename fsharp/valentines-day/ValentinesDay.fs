module ValentinesDay

type Approval = 
    | Yes
    | No
    | Maybe

type Cuisine =
    | Korean
    | Turkish

type Genre =
    | Crime
    | Horror
    | Romance
    | Thriller

type Activity = 
    | BoardGame
    | Chill
    | Movie of Genre 
    | Restaurant of Cuisine 
    | Walk of int 

let rateActivity (activity: Activity): Approval = 
    match activity with 
    | BoardGame -> No 
    | Chill -> No 
    | Movie g -> if g = Romance then Yes else No 
    | Restaurant c -> if c = Korean then Yes else Maybe 
    | Walk i -> if i < 3 then Yes elif i < 5 then Maybe else No
