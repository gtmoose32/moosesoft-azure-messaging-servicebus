using FluentAssertions;
using Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests.DelayCalculatorStrategies;

[ExcludeFromCodeCoverage]
[TestClass]
public class LinearBackOffDelayStrategyTests
{
    [TestMethod]
    public void Calculate_Test()
    {
        //Arrange
        var sut = LinearDelayCalculatorStrategy.Default;

        //Act
        var total = TimeSpan.Zero;
        for (var i = 1; i <= 10; i++)
        {
            total += sut.Calculate(i);
        }
            
        //Assert
        total.TotalMinutes.Should().Be(55);
    }
}