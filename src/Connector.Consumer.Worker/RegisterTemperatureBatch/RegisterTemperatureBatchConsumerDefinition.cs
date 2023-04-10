using MassTransit;

namespace Connector.Consumer.Worker.RegisterTemperatureBatch;

internal sealed class RegisterTemperatureBatchConsumerDefinition : ConsumerDefinition<RegisterTemperatureBatchConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<RegisterTemperatureBatchConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}
