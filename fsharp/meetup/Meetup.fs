module Meetup

open System
// use an enum-like type
type Week = 
    | First   = 1 
    | Second  = 8
    | Third   = 15
    | Fourth  = 22
    | Last    = 0
    | Teenth  = 13

let meetup (year:int) (month:int) (week:Week) (dayOfWeek:DayOfWeek): DateTime = 
    let i = if (int)week > 0 then (int)week else DateTime.DaysInMonth(year, month) - 6
    let dt = new DateTime(year, month, i)
    // AddDays takes a float
    dt.AddDays( (float) ( ( (int)dayOfWeek - (int)dt.DayOfWeek + 7 ) % 7 ) )