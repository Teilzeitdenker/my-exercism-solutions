using System;

class BirdCount
{
    private int[] birdsPerDay;

    public BirdCount(int[] birdsPerDay)
    {
        this.birdsPerDay = birdsPerDay;
    }

    public static int[] LastWeek()
    {
        return  new int[] { 0, 2, 5, 3, 7, 8, 4};
    }

    public int Today()
    {
        return this.birdsPerDay[6];
    }

    public void IncrementTodaysCount()
    {
        this.birdsPerDay[6] += 1;
    }

    public bool HasDayWithoutBirds()
    {
        foreach (int num in this.birdsPerDay)
        {
            if (num == 0) return true;
        }
        return false;
    }

    public int CountForFirstDays(int numberOfDays)
    {
        int res = 0;
        for (int i = 0; i < numberOfDays; ++i)
        {
            res += this.birdsPerDay[i];
        }
        return res;
    }

    public int BusyDays()
    {
        int num_busy = 0;
        foreach (int num in this.birdsPerDay)
        {
            if (num >= 5) num_busy += 1;
        }
        return num_busy;
    }
}
