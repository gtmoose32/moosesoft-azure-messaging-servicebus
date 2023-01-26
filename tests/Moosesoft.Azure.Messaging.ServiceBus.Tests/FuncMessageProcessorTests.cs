using System.Reflection;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class FuncMessageProcessorTests
{
    [TestMethod]
    public async Task ProcessMessageAsync_Test()
    {
        //Arrange
        var message = CreateReceivedMessage();
        var func = Substitute.For<Func<ServiceBusReceivedMessage, CancellationToken, Task>>();
        var sut = new FuncMessageProcessor(func);

        //Act
        await sut.ProcessMessageAsync(message, CancellationToken.None).ConfigureAwait(false);

        //Assert
        await func.Received(1)(Arg.Is(message), Arg.Is(CancellationToken.None)).ConfigureAwait(false);
    }

    private static ServiceBusReceivedMessage CreateReceivedMessage()
    {
        var ctor = typeof(ServiceBusReceivedMessage).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, new Type[] { });
        return ctor?.Invoke(null) as ServiceBusReceivedMessage;
    }
}