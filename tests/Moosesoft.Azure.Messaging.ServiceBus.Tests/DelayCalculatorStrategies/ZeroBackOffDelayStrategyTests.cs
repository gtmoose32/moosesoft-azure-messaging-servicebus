using Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests.DelayCalculatorStrategies;

[ExcludeFromCodeCoverage]
[TestClass]
public class ZeroBackOffDelayStrategyTests
{
    [TestMethod]
    public void Calculate_Test()
    {
        //Arrange
        var expected = TimeSpan.Zero;
        var sut = new ZeroDelayCalculatorStrategy();
        var results = new List<TimeSpan>();

        //Act
        for (var i = 1; i <= 10; i++)
        {
            results.Add(sut.Calculate(i));
        }
            
        //Assert
        Assert.IsTrue(results.All(ts => ts == expected));
    }
}