//This code is part of an imaginary home automation system.
//It is used as part of a light controller that polls a motion sensor
//and then calls the `ActuateLights` method with the results from the
//motion sensor to determine if the lights should be turned on or off.

namespace LightController;

public interface ILightSwitcher
{
    void TurnOn();

    void TurnOff();
}


public  class LightSwitcher : ILightSwitcher
{
    public void TurnOff()
    {
        Console.WriteLine("Turning off switch");
    }

    public void TurnOn()
    {
        Console.WriteLine("Turning on switch");
    }
}

public interface IMotionSensor
{
    bool DetectMotion();
}

public interface ILightActuator
{
    void ActuateLights(bool motionDetected);
}

public class LightActuator(ILightSwitcher lightSwitcher, ITimePeriodHelper timePeriodHelper) : ILightActuator
{
    // public for testing. Not part of interface
    public DateTime LastMotionTime { get; set; }

    public void ActuateLights(bool motionDetected)
    {
        DateTime time = DateTime.Now;

        // Update the time of last motion.
        if (motionDetected)
        {
            LastMotionTime = time;
        }

        // If motion was detected in the evening or at night, turn the light on.
        string timePeriod = timePeriodHelper.GetTimePeriod(time);
        if (motionDetected && (timePeriod == "Evening" || timePeriod == "Night"))
        {
            lightSwitcher.TurnOn();
        }
        // If no motion is detected for one minute, or if it is morning or day, turn the light off.
        else if (time.Subtract(LastMotionTime) > TimeSpan.FromMinutes(1) || (timePeriod == "Morning" || timePeriod == "Afternoon"))
        {
            lightSwitcher.TurnOff();
        }
    }
}

public class TimePeriodHelper : ITimePeriodHelper
{

    public string GetTimePeriod(DateTime dateTime)
    {
        if (dateTime.Hour >= 0 && dateTime.Hour < 6)
        {
            return "Night";
        }
        if (dateTime.Hour >= 6 && dateTime.Hour < 12)
        {
            return "Morning";
        }
        if (dateTime.Hour >= 12 && dateTime.Hour < 18)
        {
            return "Afternoon";
        }
        return "Evening";
    }
}
