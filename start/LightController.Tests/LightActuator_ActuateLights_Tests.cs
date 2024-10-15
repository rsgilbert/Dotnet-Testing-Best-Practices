using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightController.Tests;

public class LightActuator_ActuateLights_Tests
{
    [Fact]
    public void MotionDetected_SetCurrentDate()
    {
        // Arrange
        bool motionDetected = true;
        DateTime startTime = new DateTime(2000, 1, 1); // not now
        Mock<ILightSwitcher> lightSwitcherMock = new Mock<ILightSwitcher>();

        // Act
        LightActuator lightActuator = new(lightSwitcherMock.Object);
        lightActuator.ActuateLights(motionDetected);
        DateTime lastMotionTime = lightActuator.LastMotionTime;


        // Assert
        lastMotionTime.Should().NotBe(startTime);
    }
    public void MotionNotDetected_DonotSetCurrentDate()
    {
        throw new NotImplementedException();
    }


    [Theory]
    [InlineData("Morning", false)]
    [InlineData("Afternoon", false)]
    [InlineData("Evening", true)]
    [InlineData("Night", true)]
    public void MotionDetectedAndNight_TurnOn(string timePeriod, bool expected)
    {
        throw new NotImplementedException();
            
    }

    [Theory]
    [InlineData("Morning", true)]
    [InlineData("Afternoon", true)]
    [InlineData("Evening", false)]
    [InlineData("Night", false)]
    public void MotionNotDetectedAndNight_TurnOff(string timePeriod, bool expected)
    {
        throw new NotImplementedException();

    }


    // TODO: Timeperiod tests

}
