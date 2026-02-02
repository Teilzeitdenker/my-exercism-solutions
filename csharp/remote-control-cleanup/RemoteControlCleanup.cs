public class RemoteControlCar
{
    public string CurrentSponsor { get; private set; }
    private Speed currentSpeed;
    // must be public, since it should be accessible by car.Telemetry
    public ITelemetry Telemetry;
    // design a public interface, s.t. the actual CarTelemetry class can be made private (not accessible from outside a RemoteControlCar object).
    public interface ITelemetry
    {
        public void Calibrate();
        public bool SelfTest();
        public void ShowSponsor(string name);
        public void SetSpeed(decimal amount, string units);

    }
    // the constructur of the RemoteControlCar gives itself as a paramater to the CarTelemetry contructor
    public RemoteControlCar()
    {
        Telemetry = new CarTelemetry(this);
    }
    // implement the interface in the CarTelemetry class
    private class CarTelemetry : ITelemetry
    {
        // in order to call the methods from the RemoteControlCar have to take a "parent" paramater
        public RemoteControlCar _parent;
        public CarTelemetry(RemoteControlCar c)
        {
            _parent = c;
        }
        public void Calibrate()
        {

        }

        public bool SelfTest()
        {
            return true;
        }

        public void ShowSponsor(string sponsorName)
        {
            _parent.SetSponsor(sponsorName);
        }

        public void SetSpeed(decimal amount, string unitsString)
        {
            SpeedUnits speedUnits = SpeedUnits.MetersPerSecond;
            if (unitsString == "cps")
            {
                speedUnits = SpeedUnits.CentimetersPerSecond;
            }

            _parent.SetSpeed(new Speed(amount, speedUnits));
        }
    }

    public string GetSpeed()
    {
        return currentSpeed.ToString();
    }

    private void SetSponsor(string sponsorName)
    {
        CurrentSponsor = sponsorName;

    }

    private void SetSpeed(Speed speed)
    {
        currentSpeed = speed;
    }
    // take these into the class definition and make them private
    private enum SpeedUnits
    {
        MetersPerSecond,
        CentimetersPerSecond
    }

    private struct Speed
    {
        public decimal Amount { get; }
        public SpeedUnits SpeedUnits { get; }

        public Speed(decimal amount, SpeedUnits speedUnits)
        {
            Amount = amount;
            SpeedUnits = speedUnits;
        }

        public override string ToString()
        {
            string unitsString = "meters per second";
            if (SpeedUnits == SpeedUnits.CentimetersPerSecond)
            {
                unitsString = "centimeters per second";
            }

            return Amount + " " + unitsString;
        }
    }
}


