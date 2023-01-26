namespace Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;

/// <summary>
/// This failure policy will abandon the message for any exceptions that occur without any back off delay.
/// </summary>
public class AbandonMessageFailurePolicy : FailurePolicyBase
{
    /// <summary>
    /// Initialize a new instance <see cref="AbandonMessageFailurePolicy"/>.
    /// </summary>
    public AbandonMessageFailurePolicy() : base(_ => false)
    {
    }

    /// <inheritdoc />
    public override Task HandleFailureAsync(MessageContext messageContext, CancellationToken cancellationToken)
        => Task.CompletedTask;
}