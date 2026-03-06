public static class BafflingBirthdays
{
    public static DateOnly[] RandomBirthdates(int numberOfBirthdays)
    {
        var rd = new Random();
        var result = new DateOnly[numberOfBirthdays];
        for (int i = 0; i < numberOfBirthdays; i++) 
        {
            int year;
            do
            {
                year = rd.Next(1930, 2026);
            } while (DateTime.IsLeapYear(year));
            int month = rd.Next(1, 13);
            int day = rd.Next(1, 32);
            try
            {
                result[i] = new DateOnly(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                i--;
            }
        }
        return result;
    }

    public static bool SharedBirthday(DateOnly[] birthdays)
    {
        var seen = new HashSet<(int, int)>();
        foreach (var date in birthdays)
        {
            var pair = (date.Month, date.Day);
            if (!seen.Add(pair)) return true;
        }
        return false;
    }
    
    public static double EstimatedProbabilityOfSharedBirthday(int numberOfBirthdays)
    {
        const int numSims = 10000;
        int cnt = 0;
        for (int i = 0; i < numSims; i++)
        {
            if (SharedBirthday(RandomBirthdates(numberOfBirthdays))) cnt++;
        }
        return (cnt / (double)numSims) * 100.0;
    }
}
