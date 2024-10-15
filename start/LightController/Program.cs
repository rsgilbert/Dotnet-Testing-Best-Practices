using System.Security.Authentication.ExtendedProtection;
using Microsoft.Extensions.DependencyInjection;

namespace LightController;

public class LightController
{
    private readonly IMotionSensor MotionSensor;
    private readonly ILightActuator LightActuator;
    private readonly Timer timer;

    public LightController(IMotionSensor motionSensor, ILightActuator lightActuator)
    {
        MotionSensor = motionSensor;
        LightActuator = lightActuator;

        timer = new Timer
        {
            Enabled = true,
            Interval = 1000 // ms
        };
        timer.Elapsed += Poll;
    }

    public void Poll(object? source, EventArgs? e)
    {
        LightActuator.ActuateLights(MotionSensor.DetectMotion());
    }
}

public class MotionSensorInstance : IMotionSensor
{
    private bool motion;
    public bool DetectMotion()
    {
        motion = !motion;
        return motion;
    }
}

public class Program
{
    public static void Main()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IMotionSensor, MotionSensorInstance>();
        services.AddSingleton<ILightSwitcher, LightSwitcher>();
        services.AddSingleton<ILightActuator, LightActuator>();
        services.AddSingleton<LightController, LightController>();
        services.AddSingleton<ITimePeriodHelper, TimePeriodHelper>();

        var app = services.BuildServiceProvider();
        LightController controller = app.GetRequiredService<LightController>();
        while(true)
        {
            Thread.Sleep(100);
        }
    }
}
