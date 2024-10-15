using FluentAssertions;

namespace LightController.Tests;

public class HelloWorld
{
    [Fact]
    public void HelloTest()
    {
        true.Should().BeTrue();
    }
}
