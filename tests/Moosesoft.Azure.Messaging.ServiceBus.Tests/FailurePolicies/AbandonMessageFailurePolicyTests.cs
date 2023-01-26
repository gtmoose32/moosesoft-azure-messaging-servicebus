using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests.FailurePolicies;

[ExcludeFromCodeCoverage]
[TestClass]
public class AbandonMessageFailurePolicyTests
{
    private IFailurePolicy _sut;

    [TestInitialize]
    public void Init()
    {
        _sut = new AbandonMessageFailurePolicy();
    }

    [TestMethod]
    public void CanHandle_Test()
    {
        //Act
        var result = _sut.CanHandle(new Exception());

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task HandleFailureAsync_Test()
    {
        //Arrange
        var messageContext = Substitute.For<MessageContext>();

        //Act
        await _sut.HandleFailureAsync(messageContext, CancellationToken.None);

        //Assert
        await messageContext.DidNotReceiveWithAnyArgs()
            .AbandonMessageAsync(Arg.Any<IDictionary<string, object>>(), Arg.Any<CancellationToken>())
            .ConfigureAwait(false);

        await messageContext.DidNotReceiveWithAnyArgs()
            .CompleteMessageAsync(Arg.Any<CancellationToken>())
            .ConfigureAwait(false);

        await messageContext.DidNotReceiveWithAnyArgs()
            .DeadLetterMessageAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}