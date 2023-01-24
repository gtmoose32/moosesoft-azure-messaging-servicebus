using FluentAssertions;
// ReSharper disable ObjectCreationAsStatement

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class ServiceBusEntityDescriptionTests
{
    [TestMethod]
    public void IsQueue_Test()
    {
        //Arrange
        var sut = new ServiceBusEntityDescription("test-queue");

        //Act
        var result = sut.IsQueue();

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void QueueName_Test()
    {
        //Arrange
        const string queueName = "test-queue";
        var sut = new ServiceBusEntityDescription(queueName);

        //Act
        var result = sut.QueueName;

        //Assert
        result.Should().Be(queueName);
    }

    [TestMethod]
    public void TopicName_Test()
    {
        //Arrange
        const string topicName = "test-topic";
        var sut = new ServiceBusEntityDescription(topicName, "sub");

        //Act
        var result = sut.TopicName;

        //Assert
        result.Should().Be(topicName);
    }

    [TestMethod]
    public void SubscriptionName_Test()
    {
        //Arrange
        const string subName = "test-sub";
        var sut = new ServiceBusEntityDescription("topic", subName);

        //Act
        var result = sut.SubscriptionName;

        //Assert
        result.Should().Be(subName);
    }

    [TestMethod]
    public void Ctor_QueueNameEmpty_Test()
    {
        //Arrange
        var queueName = String.Empty;

        //Act
        Action act = () => new ServiceBusEntityDescription(queueName);

        //Assert
        act.Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("Cannot be null, empty or whitespace. (Parameter 'queueName')");
    }

    [TestMethod]
    public void Ctor_TopicNameEmpty_Test()
    {
        //Arrange
        var topicName = String.Empty;

        //Act
        Action act = () => new ServiceBusEntityDescription(topicName, "sub");

        //Assert
        act.Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("Cannot be null, empty or whitespace. (Parameter 'topicName')");
    }

    [TestMethod]
    public void Ctor_SubNameEmpty_Test()
    {
        //Arrange
        var subName = String.Empty;

        //Act
        Action act = () => new ServiceBusEntityDescription("topic", subName);

        //Assert
        act.Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("Cannot be null, empty or whitespace. (Parameter 'subscriptionName')");
    }
}