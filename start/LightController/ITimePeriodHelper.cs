//This code is part of an imaginary home automation system.
//It is used as part of a light controller that polls a motion sensor
//and then calls the `ActuateLights` method with the results from the
//motion sensor to determine if the lights should be turned on or off.


namespace LightController;

public interface ITimePeriodHelper
{
    string GetTimePeriod(DateTime dateTime);
}