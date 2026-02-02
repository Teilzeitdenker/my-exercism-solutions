using System;
using System.Runtime.InteropServices;
using System.Globalization;


public enum Location
{
    NewYork,
    London,
    Paris
}

public enum AlertLevel
{
    Early,
    Standard,
    Late
}

public static class Appointment
{
    public static DateTime ShowLocalTime(DateTime dtUtc)
    {
        TimeZoneInfo localZone = TimeZoneInfo.Local;
        return TimeZoneInfo.ConvertTimeFromUtc(dtUtc, localZone);
    }

    public static DateTime Schedule(string appointmentDateDescription, Location location)
    {
        DateTime localTime = DateTime.Parse(appointmentDateDescription);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            switch (location)
            {
                case Location.NewYork:
                    TimeZoneInfo newYork = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    return TimeZoneInfo.ConvertTimeToUtc(localTime, newYork);
                case Location.London:
                    TimeZoneInfo london = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                    return TimeZoneInfo.ConvertTimeToUtc(localTime, london);
                case Location.Paris:
                    TimeZoneInfo paris = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                    return TimeZoneInfo.ConvertTimeToUtc(localTime, paris);
                default:
                    return DateTime.MinValue;
            }
        }
        else
        {
            switch (location)
            {
                case Location.NewYork:
                    TimeZoneInfo newYork = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                    return TimeZoneInfo.ConvertTimeToUtc(localTime, newYork);
                case Location.London:
                    TimeZoneInfo london = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");
                    return TimeZoneInfo.ConvertTimeToUtc(localTime, london);
                case Location.Paris:
                    TimeZoneInfo paris = TimeZoneInfo.FindSystemTimeZoneById("Europe/Paris");
                    return TimeZoneInfo.ConvertTimeToUtc(localTime, paris);
                default:
                    return DateTime.MinValue;
            }
        }


    }

    public static DateTime GetAlertTime(DateTime appointment, AlertLevel alertLevel)
    {
        TimeSpan early = new TimeSpan(24, 0, 0);
        TimeSpan standard = new TimeSpan(1, 45, 0);
        TimeSpan late = new TimeSpan(0, 30, 0);
        switch (alertLevel)
        {
            case AlertLevel.Early:
                return appointment - early;
            case AlertLevel.Standard:
                return appointment - standard;
            case AlertLevel.Late:
                return appointment - late;
            default:
                return appointment;
        }
    }

    public static bool HasDaylightSavingChanged(DateTime dt, Location location)
    {
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            switch (location)
            {
                case Location.NewYork:
                    localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    break;
                case Location.London:
                    localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                    break;
                case Location.Paris:
                    localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (location)
            {
                case Location.NewYork:
                    localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                    break;
                case Location.London:
                    localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");
                    break;
                case Location.Paris:
                    localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Paris");
                    break;
                default:
                    break;
            }
        }
        TimeSpan week = new TimeSpan(7, 0, 0, 0);
        if (localTimeZone.IsDaylightSavingTime(dt - week) == localTimeZone.IsDaylightSavingTime(dt)) return false;
        else return true;
    }

    public static DateTime NormalizeDateTime(string dtStr, Location location)
    {
        DateTimeFormatInfo local = CultureInfo.InvariantCulture.DateTimeFormat;
        CultureInfo britain = CultureInfo.GetCultureInfo("en-GB");
        CultureInfo america = CultureInfo.GetCultureInfo("en-US");
        CultureInfo france = CultureInfo.GetCultureInfo("fr-FR");
        switch (location)
        {
            case Location.London:
                local = britain.DateTimeFormat;
                break;
            case Location.NewYork:
                local = america.DateTimeFormat;
                break;
            case Location.Paris:
                local = france.DateTimeFormat;
                break;
            default:
                break;
        }
        DateTime result;
        if (DateTime.TryParse(dtStr, local, DateTimeStyles.None, out result))
        {
            return result;
        }
        else return new DateTime(1, 1, 1);
    }
}
