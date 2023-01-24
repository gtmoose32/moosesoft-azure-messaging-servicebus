using Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;
using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Moosesoft.Azure.Messaging.ServiceBus.Builders;

[ExcludeFromCodeCoverage]
internal class BuilderState
{
    public ServiceBusEntityDescription EntityDescription { get; }
    public IDelayCalculatorStrategy DelayCalculatorStrategy { get; set; }
    public Func<IFailurePolicy> FailurePolicyFactory { get; set; }
    public IMessageProcessor MessageProcessor { get; set; }

    public BuilderState(ServiceBusEntityDescription entityDescription)
    {
        EntityDescription = entityDescription ?? throw new ArgumentNullException(nameof(entityDescription));
    }
}