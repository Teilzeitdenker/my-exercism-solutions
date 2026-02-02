using System;
using System.Collections.Generic;

public interface IRemoteControlCar
{
    int DistanceTravelled { get; }
    void Drive();
}

public class ProductionRemoteControlCar : IRemoteControlCar, IComparable<ProductionRemoteControlCar>
{
    public int DistanceTravelled { get; private set; }
    public int NumberOfVictories { get; set; }

    public void Drive()
    {
        DistanceTravelled += 10;
    }
    
    int IComparable<ProductionRemoteControlCar>.CompareTo(ProductionRemoteControlCar other)
    {
        if (this.NumberOfVictories > other.NumberOfVictories) return 1;
        if (this.NumberOfVictories < other.NumberOfVictories) return -1;
        return 0;
    }
}

public class ExperimentalRemoteControlCar : IRemoteControlCar
{
    public int DistanceTravelled { get; private set; }

    public void Drive()
    {
        DistanceTravelled += 20;
    }
}

public static class TestTrack
{
    public static void Race(IRemoteControlCar car)
    {
        car.Drive();
    }

    public static List<ProductionRemoteControlCar> GetRankedCars(ProductionRemoteControlCar prc1,
        ProductionRemoteControlCar prc2)
    {
        var l = new List<ProductionRemoteControlCar> { prc1, prc2 };
        l.Sort();
        return l;
    }
}
