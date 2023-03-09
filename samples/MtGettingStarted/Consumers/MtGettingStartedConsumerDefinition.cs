namespace Company.Consumers;

using MassTransit;

public class MtGettingStartedConsumerDefinition : ConsumerDefinition<MtGettingStartedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<MtGettingStartedConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}
