using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightController.Tests;

public class LightActuator_ActuateLights_Tests
{
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void MotionDetected_SetCurrentDate(bool motionDetected, bool shouldSetCurrentDate)
    {
        // Arrange
        DateTime startTime = new(2000, 1, 1); // not now
        Mock<ILightSwitcher> lightSwitcherMock = new Mock<ILightSwitcher>();
        var ioc = new AutoMocker();

        // Act
        LightActuator lightActuator = ioc.CreateInstance<LightActuator>();
        lightActuator.LastMotionTime = startTime;
        lightActuator.ActuateLights(motionDetected);
        DateTime lastMotionTime = lightActuator.LastMotionTime;


        // Assert
        if (shouldSetCurrentDate)
        {
            lastMotionTime.Should().NotBe(startTime);
        }
        else
        {
            lastMotionTime.Should().Be(startTime);
        }
    }
   

    
    [Theory]
    [InlineData(TimePeriod.Morning, false)]
    [InlineData(TimePeriod.Afternoon, false)]
    [InlineData(TimePeriod.Evening, true)]
    [InlineData(TimePeriod.Night, true)]
    public void MotionDetectedAndNight_TurnOn(TimePeriod timePeriod, bool expectedTurnOn)
    {
        // Arrange
        bool actualTurnOn= false;
        bool motionDetected = true;
        DateTime startTime = new DateTime(2000, 1, 1); // not now
        var ioc = new AutoMocker();
        Mock<ILightSwitcher> lightSwitcherMock = ioc.GetMock<ILightSwitcher>();
        lightSwitcherMock.Setup(m => m.TurnOn()).Callback(() =>
        {
            actualTurnOn = true;
        });
        Mock<ITimePeriodHelper> timePeriodMock = ioc.GetMock<ITimePeriodHelper>();
        timePeriodMock.Setup(m => m.GetTimePeriod(It.IsAny<DateTime>())).Returns(timePeriod);
        // Act
        LightActuator lightActuator = ioc.CreateInstance<LightActuator>();
        lightActuator.ActuateLights(motionDetected);


        // Assert
        actualTurnOn.Should().Be(expectedTurnOn);

    }

    [Theory]
    [InlineData(TimePeriod.Morning, true)]
    [InlineData(TimePeriod.Afternoon, true)]
    [InlineData(TimePeriod.Evening, false)]
    [InlineData(TimePeriod.Night, false)]
    public void MotionNotDetectedAndDay_TurnOff(TimePeriod timePeriod, bool expectedTurnOff)
    {
        // Arrange
        bool actualTurnOff = false;
        bool motionDetected = false;
        DateTime startTime = DateTime.Now.Subtract(TimeSpan.FromSeconds(1)); // 1 second ago
        var ioc = new AutoMocker();
        Mock<ILightSwitcher> lightSwitcherMock = ioc.GetMock<ILightSwitcher>();
        lightSwitcherMock.Setup(m => m.TurnOff()).Callback(() =>
        {
            actualTurnOff = true;
        });
        Mock<ITimePeriodHelper> timePeriodMock = ioc.GetMock<ITimePeriodHelper>();
        timePeriodMock.Setup(m => m.GetTimePeriod(It.IsAny<DateTime>())).Returns(timePeriod);
        // Act
        LightActuator lightActuator = ioc.CreateInstance<LightActuator>();
        lightActuator.LastMotionTime = startTime;
        lightActuator.ActuateLights(motionDetected);


        // Assert
        actualTurnOff.Should().Be(expectedTurnOff);

    }


    // TODO: Timeperiod tests

}
