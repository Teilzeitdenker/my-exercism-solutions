using System;
using System.Text.RegularExpressions;

public static class SwiftScheduling
{
    private static readonly Regex DescriptionRegex = // use named groups to capture different formats
        new(@"^(?:(?<fixed>NOW|ASAP|EOW)|(?<month>\d+)M|Q(?<quarter>\d+))$", 
            RegexOptions.Compiled);
    
    public static DateTime DeliveryDate(DateTime meetingStart, string description) 
    {
        var match = DescriptionRegex.Match(description);
        
        if (!match.Success) throw new ArgumentException(description);
        
        if (match.Groups["month"].Success)
        {
            int month = int.Parse(match.Groups["month"].Value);
            return GetMonthDeliveryDate(meetingStart, month);
        }
        
        if (match.Groups["quarter"].Success)
        {
            int quarter = int.Parse(match.Groups["quarter"].Value);
            return GetQuarterDeliveryDate(meetingStart, quarter);
        }
        
        string fixedType = match.Groups["fixed"].Value;
        return fixedType switch
        {
            "NOW"  => meetingStart.AddHours(2),
            "ASAP" => GetASAPDeliveryDate(meetingStart),
            "EOW"  => GetEOWDeliveryDate(meetingStart),
            _      => throw new ArgumentException()
        };
    }

    private static DateTime GetQuarterDeliveryDate(DateTime meetingStart, int quarter)
    {
        int currentYear = meetingStart.Year;
        int currentMonth = meetingStart.Month;
        int currentQuarter = (currentMonth - 1) / 3 + 1;
        
        int deliveryYear = currentQuarter > quarter ? currentYear + 1 : currentYear;
        int lastMonthOfQuarter = quarter * 3;
        int daysInMonth = DateTime.DaysInMonth(deliveryYear, lastMonthOfQuarter);
        DateTime lastDay = new DateTime(deliveryYear, lastMonthOfQuarter, daysInMonth);
        
        while (lastDay.DayOfWeek == DayOfWeek.Saturday || lastDay.DayOfWeek == DayOfWeek.Sunday)
            lastDay = lastDay.AddDays(-1);
        return lastDay.Date.AddHours(8);
    }
    
    private static DateTime GetMonthDeliveryDate(DateTime meetingStart, int month)
    {
        int currentYear = meetingStart.Year;
        int currentMonth = meetingStart.Month;
        
        int deliveryYear = currentMonth >= month ? currentYear + 1 : currentYear;
        DateTime firstDay = new DateTime(deliveryYear, month, 1);
        
        while (firstDay.DayOfWeek == DayOfWeek.Saturday || firstDay.DayOfWeek == DayOfWeek.Sunday)
            firstDay = firstDay.AddDays(1);
        return firstDay.Date.AddHours(8);
    }
    
    private static DateTime GetASAPDeliveryDate(DateTime meetingStart) =>
        meetingStart.Hour < 13 ? 
        meetingStart.Date.AddHours(17) : 
        meetingStart.Date.AddDays(1).AddHours(13);
    
    private static DateTime GetEOWDeliveryDate(DateTime meetingStart) =>
        (meetingStart.DayOfWeek >= DayOfWeek.Monday && meetingStart.DayOfWeek <= DayOfWeek.Wednesday) ?
        meetingStart.Date.AddDays((int)DayOfWeek.Friday - (int)meetingStart.DayOfWeek).AddHours(17) :
        meetingStart.Date.AddDays(7 - (int)meetingStart.DayOfWeek).AddHours(20);
}
