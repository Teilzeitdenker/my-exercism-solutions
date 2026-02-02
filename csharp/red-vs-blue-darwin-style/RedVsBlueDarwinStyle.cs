namespace RedRemoteControlCarTeam
{
    public class RemoteControlCar
    {
        public RemoteControlCar(Motor motor, Chassis chassis, Telemetry telemetry, RunningGear runningGear)
        {
        }
        // red members and API
    }

    public class RunningGear
    {
        // red members and API
    }

    public class Telemetry
    {
        // red members and API
    }

    public class Chassis
    {
        // red members and API
    }

    public class Motor
    {
        // red members and API
    }
}

namespace BlueRemoteControlCarTeam
{
    public class RemoteControlCar
    {
        public RemoteControlCar(Motor motor, Chassis chassis, Telemetry telemetry)
        {
        }
        // blue members and API
    }

    public class Telemetry
    {
        // blue members and API
    }

    public class Chassis
    {
        // blue members and API
    }

    public class Motor
    {
        // blue members and API
    }
}

namespace Combined
{
    using R = RedRemoteControlCarTeam;
    using B = BlueRemoteControlCarTeam;
    public class CarBuilder
    {
        public static R.RemoteControlCar BuildRed()
        {
            return new R.RemoteControlCar(
                new R.Motor(),
                new R.Chassis(),
                new R.Telemetry(),
                new R.RunningGear()
            );
        }

        public static B.RemoteControlCar BuildBlue()
        {
            return new B.RemoteControlCar(
                new B.Motor(),
                new B.Chassis(),
                new B.Telemetry()
            );
        }
    }
}
