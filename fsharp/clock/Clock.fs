module Clock

type Clock = { 
    Hours: int 
    Minutes: int 
}

// Well, % in C-derived languages is not the modulo operator but the remainder operator, so it might be negative!
// This is quite unlike in Python

// I break the add-method down in two parts, first add the additional hours (and set the Minutes to 0)
let addHours (hours:int) (clock:Clock) : Clock = 
    let modulo = (hours + clock.Hours) % 24
    if hours >= -clock.Hours then
        { Hours = modulo; Minutes = 0 }
    else 
        { Hours = 24 + modulo; Minutes = 0 }
// then come the remaining minutes
let addMinutes (minutes:int) (clock:Clock) : Clock =
    // minutes in this routine should be an integer between -60 and 60
    if minutes < -60 || minutes > 60 then
        invalidArg "minutes" "trying to add/subtract more than 60 minutes"
    elif clock.Minutes + minutes > 60 then 
        let rightHour = addHours 1 clock 
        { rightHour with Minutes = (clock.Minutes + minutes) % 60}
    elif clock.Minutes + minutes < 0 then 
        let rightHour = addHours -1 clock 
        { rightHour with Minutes = clock.Minutes + minutes + 60}
    else
        {clock with Minutes = clock.Minutes + minutes }

// this is quite straightforward now and shows the general idea I had
let add minutes (clock:Clock) : Clock = 
    let additionalHours = (clock.Minutes + minutes) / 60
    let remainingMinutes = (clock.Minutes + minutes) % 60
    clock |> addHours additionalHours |> addMinutes remainingMinutes

// well, easy peasy...
let subtract minutes = add (-minutes)

// a little helper method for creation
let getRightHour (h:int) = 
    if h >= 0 then 
        h % 24 
    else 
        24 + (h % 24)

// This is done with recursion (at most 1 recursive invocation)
let rec create hours minutes : Clock = 
    if minutes = 0 then 
        { Hours = getRightHour hours; Minutes = 0 }
    else 
        add minutes (create hours 0)

// Can't figure out how to do it with format strings, so found this "little" workaround.
// Of course I know it's not elegant at all to instantiate these 
// big DateTime-Objects just to find out the clock display.
let display (clock:Clock) = System.DateTime(1000,1,1,clock.Hours, clock.Minutes, 0).ToShortTimeString()