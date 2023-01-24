using FluentAssertions;
using Moosesoft.Azure.Messaging.ServiceBus.Builders;
using Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;
using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;
using Moosesoft.Azure.Messaging.ServiceBus.Tests.Infrastructure;
using NSubstitute;
// ReSharper disable PossibleNullReferenceException
// ReSharper disable UnusedParameter.Local
// ReSharper disable ExpressionIsAlwaysNull

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests.Builders;

[ExcludeFromCodeCoverage]
[TestClass]
public class MessageContextProcessorBuilderTests
{
    private readonly ServiceBusEntityDescription _entityDescription = new("test-queue");

    [TestMethod]
    public void Configure_Test()
    {
        //Arrange

        //Act
        var result = MessageContextProcessorBuilder.Configure(_entityDescription);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<MessageContextProcessorBuilder>();

        var state = ((MessageContextProcessorBuilder)result).GetBuilderState();
        state.Should().NotBeNull();
        state.EntityDescription.Should().Be(_entityDescription);
    }

    [TestMethod]
    public void WithMessageProcessor_ArgumentNull_Test()
    {
        //Arrange
        IMessageProcessor processor = null;
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        Action act = () => sut.WithMessageProcessor(processor);

        //Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [TestMethod]
    public void WithMessageProcessor_Test()
    {
        //Arrange
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        var result = sut.WithMessageProcessor<TestMessageProcessor>();

        //Assert
        result.Should().NotBeNull();
        var state = ((MessageContextProcessorBuilder)result).GetBuilderState();
        state.Should().NotBeNull();
        state.MessageProcessor.Should().BeOfType<TestMessageProcessor>();
    }

    [TestMethod]
    public void WithMessageProcessor_Func_Test()
    {
        //Arrange
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        var result = sut.WithMessageProcessor((message, token) => Task.CompletedTask);

        //Assert
        result.Should().NotBeNull();
        var state = ((MessageContextProcessorBuilder)result).GetBuilderState();
        state.Should().NotBeNull();
        state.MessageProcessor.Should().BeOfType<FuncMessageProcessor>();
    }

    [TestMethod]
    public void WithDelayCalculatorStrategy_ArgumentNull_Test()
    {
        //Arrange
        IDelayCalculatorStrategy strategy = null;
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        Action act = () => sut.WithDelayCalculatorStrategy(strategy);

        //Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }
    
    [TestMethod]
    public void WithDelayCalculatorStrategy_Test()
    {
        //Arrange
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        var result = sut.WithDelayCalculatorStrategy<TestDelayCalculatorStrategy>();

        //Assert
        result.Should().NotBeNull();
        var state = ((MessageContextProcessorBuilder)result).GetBuilderState();
        state.Should().NotBeNull();
        state.DelayCalculatorStrategy.Should().BeOfType<TestDelayCalculatorStrategy>();
    }

    [TestMethod]
    public void WithExponentialDelayCalculatorStrategy_Test()
    {
        //Arrange
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        var result = sut.WithExponentialDelayCalculatorStrategy();

        //Assert
        result.Should().NotBeNull();
        var state = ((MessageContextProcessorBuilder)result).GetBuilderState();
        state.Should().NotBeNull();
        state.DelayCalculatorStrategy.Should().BeOfType<ExponentialDelayCalculatorStrategy>();
    }

    [TestMethod]
    public void WithFixedDelayCalculatorStrategy_Test()
    {
        //Arrange
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        var result = sut.WithFixedDelayCalculatorStrategy();

        //Assert
        result.Should().NotBeNull();
        var state = ((MessageContextProcessorBuilder)result).GetBuilderState();
        state.Should().NotBeNull();
        state.DelayCalculatorStrategy.Should().BeOfType<FixedDelayCalculatorStrategy>();
    }

    [TestMethod]
    public void WithLinearDelayCalculatorStrategy_Test()
    {
        //Arrange
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        var result = sut.WithLinearDelayCalculatorStrategy();

        //Assert
        result.Should().NotBeNull();
        var state = ((MessageContextProcessorBuilder)result).GetBuilderState();
        state.Should().NotBeNull();
        state.DelayCalculatorStrategy.Should().BeOfType<LinearDelayCalculatorStrategy>();
    }

    [TestMethod]
    public void WithZeroDelayCalculatorStrategy_Test()
    {
        //Arrange
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        var result = sut.WithZeroDelayCalculatorStrategy();

        //Assert
        result.Should().NotBeNull();
        var state = ((MessageContextProcessorBuilder)result).GetBuilderState();
        state.Should().NotBeNull();
        state.DelayCalculatorStrategy.Should().BeOfType<ZeroDelayCalculatorStrategy>();
    }

    [TestMethod]
    public void WithAbandonMessageFailurePolicy_Test()
    {
        //Arrange
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        var result = sut.WithAbandonMessageFailurePolicy();

        //Assert
        result.Should().NotBeNull();
        var state = ((MessageContextProcessorBuilder)result).GetBuilderState();
        state.Should().NotBeNull();
        state.FailurePolicyFactory.Should().NotBeNull();
        var policy = state.FailurePolicyFactory();
        policy.Should().NotBeNull();
        policy.Should().BeOfType<AbandonMessageFailurePolicy>();
    }

    [TestMethod]
    public void WithCloneFailurePolicy_Test()
    {
        //Arrange
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        var result = sut.WithCloneMessageFailurePolicy();

        //Assert
        result.Should().NotBeNull();
        var state = ((MessageContextProcessorBuilder)result).GetBuilderState();
        state.Should().NotBeNull();
        state.FailurePolicyFactory.Should().NotBeNull();
        var policy = state.FailurePolicyFactory();
        policy.Should().NotBeNull();
        policy.Should().BeOfType<CloneMessageFailurePolicy>();
    }

    [TestMethod]
    public void WithFailurePolicy_ArgumentNull_Test()
    {
        //Arrange
        IFailurePolicy policy = null;
        var sut = new MessageContextProcessorBuilder(_entityDescription);

        //Act
        Action act = () => sut.WithFailurePolicy(policy);

        //Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [TestMethod]
    public void Build_Test()
    {
        //Arrange
        var sut = new MessageContextProcessorBuilder(_entityDescription);
        sut.GetBuilderState().MessageProcessor = Substitute.For<IMessageProcessor>();
        sut.GetBuilderState().FailurePolicyFactory = () => Substitute.For<IFailurePolicy>();

        //Act
        var result = sut.Build();

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ServiceBusReceivedMessageContextProcessor>();
    }

    [TestMethod]
    public void DefaultCanHandle_Test()
    {
        //Arrange

        //Act
        var result = MessageContextProcessorBuilder.DefaultCanHandle;

        //Assert
        result.Should().NotBeNull();
        result(new Exception()).Should().BeTrue();
    }
}