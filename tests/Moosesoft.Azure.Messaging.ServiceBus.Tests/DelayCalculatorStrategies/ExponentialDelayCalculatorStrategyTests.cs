using Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests.DelayCalculatorStrategies;

[ExcludeFromCodeCoverage]
[TestClass]
public class ExponentialDelayCalculatorStrategyTests
{
    [TestMethod]
    public void Calculate_Test()
    {
        //Arrange
        var sut = ExponentialDelayCalculatorStrategy.Default;
        var results = new List<TimeSpan>();

        //Act
        for (var i = 1; i <= 10; i++)
        {
            results.Add(sut.Calculate(i));
        }
            
        //Assert
        var hour = TimeSpan.FromHours(1);
        for (var i = 0; i < 10; i++)
        {
            if (i < 5)
                Assert.IsTrue(TimeSpan.Zero < results[i] && hour > results[i]);
            else
                Assert.IsTrue(hour == results[i]);
        }
    }

    [TestMethod]
    [DataRow(10, new[] { 10, 40, 90, 160, 250, 360 })]
    [DataRow(100, new[] { 100, 400, 900, 1600, 2500, 3600 })]
    public void Calculate_CustomInitialDelaySeconds_Test(int initialDelay, int[] delays)
    {
        // Arrange
        var sut = new ExponentialDelayCalculatorStrategy(TimeSpan.FromHours(1), TimeSpan.FromSeconds(initialDelay));
        var results = new List<int>();

        // Act
        for (var i = 1; i <= 6; i++)
        {
            results.Add((int)sut.Calculate(i).TotalSeconds);
        }

        // Assert
        results.Should().BeEquivalentTo(delays);
    }

    [TestMethod]
    public void Calculate_InitialDelayOfZeroUsesDefault_Test()
    {
        // Arrange
        var sut = new ExponentialDelayCalculatorStrategy(TimeSpan.FromHours(1), TimeSpan.Zero);
        var results = new List<int>();

        // Act
        for (var i = 1; i <= 6; i++)
        {
            results.Add((int)sut.Calculate(i).TotalSeconds);
        }

        // Assert
        results.Should().BeEquivalentTo(new[] { 100, 400, 900, 1600, 2500, 3600 });
    }
}