public static class BafflingBirthdays
{    
    public static DateOnly[] RandomBirthdates(int num) => // just use 2025 for the year
        Enumerable.Range(0, num).Select(_ => { 
            var month = Random.Shared.Next(1, 13);
            return new DateOnly(2025, month, Random.Shared.Next(1, DateTime.DaysInMonth(2025, month) + 1));
        }).ToArray();        
    
    public static bool SharedBirthday(DateOnly[] birthdays) =>
        birthdays.Select(b => (b.Month, b.Day)).GroupBy(b => b).Any(g => g.Count() > 1);

    public static double EstimatedProbabilityOfSharedBirthday(int numberOfBirthdays) =>
        (Enumerable.Range(0, 10000).Count(_ => SharedBirthday(RandomBirthdates(numberOfBirthdays))) / 
        (double)10000 ) * 100.0;
}
