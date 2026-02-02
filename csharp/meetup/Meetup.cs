using System;

public enum Schedule
{
    Teenth = 13,
    First = 1,
    Second = 8,
    Third = 15,
    Fourth = 22,
    Last = 0
}

public class Meetup
{
    private int _month;
    private int _year;
    public Meetup(int month, int year)
    {
        _month = month;
        _year = year;
    }

    public DateTime Day(DayOfWeek dayOfWeek, Schedule schedule)
    {
        int i = (int)schedule > 0 ? (int)schedule : DateTime.DaysInMonth(_year, _month) - 6;
        DateTime dt = new DateTime(_year, _month, i);
        return dt.AddDays((dayOfWeek - dt.DayOfWeek + 7) % 7);
    }
}