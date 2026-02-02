using System;

class RemoteControlCar
{
    int speed = default;
    int batteryDrain = default;
    int meters = 0;
    int battery = 100;
    public RemoteControlCar(int speed, int batteryDrain)
    {
        this.speed = speed;
        this.batteryDrain = batteryDrain;
    }

    public bool BatteryDrained()
    {
        return this.battery < this.batteryDrain;
    }

    public int DistanceDriven()
    {
        return this.meters;
    }

    public void Drive()
    {
        if (!this.BatteryDrained())
        {
            this.meters += this.speed;
            this.battery -= this.batteryDrain;
        } 
    }

    public static RemoteControlCar Nitro()
    {
        return new RemoteControlCar(50, 4);
    }
}

class RaceTrack
{
    int distance;

    public RaceTrack(int distance)
    {
        this.distance = distance;
    }

    public bool TryFinishTrack(RemoteControlCar car)
    {
        while (car.DistanceDriven() < this.distance && !car.BatteryDrained())
        {
            car.Drive();
        }
        return car.DistanceDriven() >= this.distance;
    }
}
