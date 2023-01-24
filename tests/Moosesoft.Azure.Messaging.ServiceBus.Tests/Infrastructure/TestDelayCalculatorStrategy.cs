using Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests.Infrastructure;

[ExcludeFromCodeCoverage]
public class TestDelayCalculatorStrategy : IDelayCalculatorStrategy
{
    public TimeSpan Calculate(int attempts)
    {
        return TimeSpan.Zero;
    }
}