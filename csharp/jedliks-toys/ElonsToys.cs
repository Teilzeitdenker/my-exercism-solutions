using System;

class RemoteControlCar
{
    public int driven_meters = 0;
    public int battery_left = 100;
    public static RemoteControlCar Buy()
    {
        return new RemoteControlCar();
    }

    public string DistanceDisplay()
    {
        return string.Format("Driven {0} meters", driven_meters);
    }

    public string BatteryDisplay()
    {
        if (battery_left > 0)
        {
            return string.Format("Battery at {0}%", battery_left);
        }
        else
        {
            return "Battery empty";
        }
    }

    public void Drive()
    {
        if (battery_left > 0)
        {
            battery_left -= 1;
            driven_meters += 20;
        }
    }
}
