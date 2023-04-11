using Connector.Common.MessageBus.Contracts;
using Connector.Common.MessageBus.Contracts.Headers;
using Connector.Consumer.Worker.Infra.InfluxDb;
using Connector.Consumer.Worker.Infra.Model;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Connector.Consumer.Worker.RegisterTemperatureBatch;

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

        var headers = context.Headers;
        var fogNodeMetada = new FogNodeMetadata(
            headers.Get<string>(HeaderKeys.FogNodeId) ?? string.Empty,
            headers.Get<string>(HeaderKeys.FogNodeName) ?? string.Empty
        );

        _repository.AddRange(fogNodeMetada, temperatures);

        _logger.LogInformation(
            "Inserted new {count} items",
            temperatures.Length
        );

        return Task.CompletedTask;
    }
}
