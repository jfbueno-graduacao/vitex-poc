namespace Company.Consumers;

using System.Threading.Tasks;
using MassTransit;
using Contracts;
using Microsoft.Extensions.Logging;

public class MtGettingStartedConsumer : IConsumer<HelloMessage>
{
    private readonly ILogger<MtGettingStartedConsumer> _logger;

    public MtGettingStartedConsumer(ILogger<MtGettingStartedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<HelloMessage> context)
    {
        _logger.LogInformation("Hello {Name}", context.Message.Name);
        return Task.CompletedTask;
    }
}
