public static class BafflingBirthdays
{
    private static DateOnly RandomDateIn(int year)
    {
        var month = Random.Shared.Next(1, 13);
        var day = Random.Shared.Next(1, DateTime.DaysInMonth(year, month) + 1);
        return new DateOnly(year, month, day);
    }
        
    public static DateOnly[] RandomBirthdates(int num) => // just use 2025 for the year
        Enumerable.Range(0, num).Select(_ => RandomDateIn(2025)).ToArray();        
    
    public static bool SharedBirthday(DateOnly[] birthdays)
    {
        var seen = new HashSet<(int, int)>();
        foreach (var date in birthdays)
            if (!seen.Add((date.Month, date.Day))) 
                return true;
        return false;
    }
    
    public static double EstimatedProbabilityOfSharedBirthday(int numberOfBirthdays) =>
        (Enumerable.Range(0, 10000).Count(_ => SharedBirthday(RandomBirthdates(numberOfBirthdays))) / 
        (double)10000 ) * 100.0;
}
