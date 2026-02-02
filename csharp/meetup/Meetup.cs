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
        int i = (int)schedule;
        if (i != 0)
        {
            for (int j = 0; j < 7; j++)
            {
                DateTime dt = new DateTime(_year, _month, i + j);
                if (dt.DayOfWeek == dayOfWeek) return dt;
            }
        }
        else
        {
            int numDays = DateTime.DaysInMonth(_year, _month);
            for (int j = 0; j < 7; j++)
            {
                DateTime dt = new DateTime(_year, _month, numDays - j);
                if (dt.DayOfWeek == dayOfWeek) return dt;
            }
        }
        throw new ArgumentException();
    }
}