using Integrador.Common.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Integrador.Consumidor.Worker;

internal sealed class HelloMessageConsumer : IConsumer<HeartRateToInsertMessage>
{
    private readonly ILogger<HelloMessageConsumer> _logger;

    public HelloMessageConsumer(ILogger<HelloMessageConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<HeartRateToInsertMessage> context)
    {
        _logger.LogInformation(
            "Well, we found something {Id} - {Value}", 
            context.Message.Id, context.Message.Value
        );
        return Task.CompletedTask;
    }
}

