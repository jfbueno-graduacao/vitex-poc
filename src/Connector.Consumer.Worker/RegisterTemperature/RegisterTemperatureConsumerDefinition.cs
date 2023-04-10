using MassTransit;

namespace Connector.Consumer.Worker.RegisterTemperature;

internal sealed class RegisterTemperatureConsumerDefinition : ConsumerDefinition<RegisterTemperatureConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
        IConsumerConfigurator<RegisterTemperatureConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}