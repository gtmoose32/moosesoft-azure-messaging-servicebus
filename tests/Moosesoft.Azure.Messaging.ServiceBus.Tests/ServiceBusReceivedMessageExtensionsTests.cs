namespace Moosesoft.Azure.Messaging.ServiceBus.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class ServiceBusReceivedMessageExtensionsTests
{
    [TestMethod]
    public void CreateMessageContext_Test()
    {
        //Arrange
        var sut = ServiceBusReceivedMessageFactory.Create();
        
        //Act
        var result = sut.CreateMessageContext(Substitute.For<ServiceBusReceiver>(), Substitute.For<ServiceBusClient>());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ServiceBusReceivedMessageContext>();
    }
}