using MassTransit;

namespace Integrador.Consumidor.Worker;

internal sealed class RegisterTemperatureConsumerDefinition : ConsumerDefinition<RegisterTemperatureConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
        IConsumerConfigurator<RegisterTemperatureConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}