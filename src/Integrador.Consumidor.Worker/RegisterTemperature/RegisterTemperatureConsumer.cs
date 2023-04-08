using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Integrador.Common.MessageBus.Contracts;
using Integrador.Consumidor.Worker.Infra;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Integrador.Consumidor.Worker.RegisterTemperature;

internal sealed class RegisterTemperatureConsumer : IConsumer<TemperatureToInsertMessage>
{
    private readonly ILogger<RegisterTemperatureConsumer> _logger;
    private readonly InfluxDbSettings _influxDbSettings;

    public RegisterTemperatureConsumer(
        ILogger<RegisterTemperatureConsumer> logger,
        IOptions<InfluxDbSettings> optionsInfluxDbSettings
    )
    {
        _logger = logger;
        _influxDbSettings = optionsInfluxDbSettings.Value;
    }

    public Task Consume(ConsumeContext<TemperatureToInsertMessage> context)
    {
        using var client = new InfluxDBClient(
            url: _influxDbSettings.Host,
            token: _influxDbSettings.Token
        );

        var message = context.Message;

        var point = PointData
            .Measurement("temperature")
            .Tag("personId", message.PersonId.ToString())
            .Field("value", message.Value)
            .Timestamp(message.Timestamp, WritePrecision.S);

        using var writeApi = client.GetWriteApi();

        var lp = point.ToLineProtocol();
        _logger.LogInformation("Line protocol to send: {LineProtocol}", lp);

        writeApi.WriteRecord(
            record: lp, 
            bucket: _influxDbSettings.Bucket, 
            org: _influxDbSettings.Organization, 
            precision: WritePrecision.S
        );

        _logger.LogInformation(
            "Registered temperature with id {Id} and value {Value}", 
            message.Id, message.Value
        );

        return Task.CompletedTask;
    }
}

