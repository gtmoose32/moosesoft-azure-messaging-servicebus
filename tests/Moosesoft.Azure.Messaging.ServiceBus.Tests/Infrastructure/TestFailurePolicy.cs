using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class TestFailurePolicy : IFailurePolicy
    {
        public bool CanHandle(Exception exception)
        {
            throw new NotImplementedException();
        }

        public Task HandleFailureAsync(MessageContextBase messageContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void SetEntityDescription(ServiceBusEntityDescription description)
        {
            throw new NotImplementedException();
        }
    }
}