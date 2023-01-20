using Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;
using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;
using System;

namespace Moosesoft.Azure.Messaging.ServiceBus.Builder;

internal class BuilderState
{
    public IDelayCalculatorStrategy DelayCalculatorStrategy { get; set; }
    public Func<IFailurePolicy> FailurePolicyFactory { get; set; }
    public Type FailurePolicyType { get; set; }
    public Func<Exception, bool> CanHandle { get; set; }
    public IMessageProcessor MessageProcessor { get; set; }
}