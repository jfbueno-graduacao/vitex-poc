using Integrador.Common.MessageBus.Contracts;
using Integrador.Consumidor.Worker.Infra.InfluxDb;
using Integrador.Consumidor.Worker.Infra.Model;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Integrador.Consumidor.Worker.RegisterTemperatureBatch;

internal sealed class RegisterTemperatureBatchConsumer : IConsumer<TemperatureBatchToInsertMessage>
{
    private readonly ILogger<RegisterTemperatureBatchConsumer> _logger;
    private readonly TemperatureRepository _repository;

    public RegisterTemperatureBatchConsumer(
        ILogger<RegisterTemperatureBatchConsumer> logger,
        TemperatureRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public Task Consume(ConsumeContext<TemperatureBatchToInsertMessage> context)
    {
        _logger.LogInformation(
            "Received {count} items to insert", 
            context.Message.TemperatureBatch.Length
        );

        var temperatures = context.Message.TemperatureBatch.Select(t => new Temperature(
            t.Id, t.PersonId, t.Value, t.Timestamp
        )).ToArray();

        _repository.AddRange(temperatures);

        _logger.LogInformation(
            "Inserted new {count} items",
            temperatures.Length
        );

        return Task.CompletedTask;
    }
}
