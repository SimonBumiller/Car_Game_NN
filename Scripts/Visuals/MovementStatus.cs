using System;

public class Movement
{
    public enum Acceleration
    {
        Breaking,

        Accelerating
    }

    public enum Speed
    {
        FastForward,

        SlowForward,

        Still,

        SlowBackwards,

        FastBackwards
    }

    public static string nameFor(Acceleration a)
    {
        switch (a)
        {
            case Acceleration.Accelerating: return "Accelerating";
            case Acceleration.Breaking: return "Breaking";
        }

        throw new NotImplementedException("No name implemented for Acceleration '" + a + "'");
    }

    public static string nameFor(Speed s)
    {
        switch (s)
        {
            case Speed.Still: return "Still";
            case Speed.FastBackwards: return "Fast Backwards";
            case Speed.SlowBackwards: return "Slow Backwards";
            case Speed.FastForward: return "Fast Forward";
            case Speed.SlowForward: return "Slow Forward";
        }

        throw new NotImplementedException("No name implemented for Speed '" + s + "'");
    }

    private class AccelerationHelper
    {
        public static Acceleration from(float input)
        {
            return input > 0 ? Acceleration.Accelerating : Acceleration.Breaking;
        }
    }

    private class Speedhelper
    {
        public static Speed from(float speed, float maxSpeed, float minSpeed)
        {
            if (speed == 0) return Speed.Still;
            if (speed > maxSpeed / 2) return Speed.FastForward;
            if (speed < maxSpeed / 2 && speed < 0) return Speed.SlowForward;
            if (speed < minSpeed / 2) return Speed.FastBackwards;
            if (speed > minSpeed / 2 && speed > 0) return Speed.SlowBackwards;

            throw new ArgumentException("No Speed can be found found for max = " + maxSpeed + " min = " +
                                        minSpeed + " current = " + speed);
        }
    }
}