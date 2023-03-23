using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Integrador.Common.Contracts;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Integrador.Consumidor.Worker;

internal sealed class RegisterTemperatureConsumer : IConsumer<TemperatureToInsertMessage>
{
    private readonly ILogger<RegisterTemperatureConsumer> _logger;
    private readonly IConfiguration _configuration;

    public RegisterTemperatureConsumer(
        ILogger<RegisterTemperatureConsumer> logger,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task Consume(ConsumeContext<TemperatureToInsertMessage> context)
    {
        string token = _configuration["InfluxDbConfig:Token"] 
            ?? throw new InvalidOperationException("InfluxDB token not found");

        string bucket = _configuration["InfluxDbConfig:Bucket"] 
            ?? throw new InvalidOperationException("InfluxDB bucket not found");
            
        string org = _configuration["InfluxDbConfig:Organization"] 
            ?? throw new InvalidOperationException("InfluxDB org not found");

        string influxDbUrl = _configuration["InfluxDbConfig:Host"] 
            ?? throw new InvalidOperationException("InfluxDB host url not found");

        using var client = new InfluxDBClient(
            url: influxDbUrl,
            token: token
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

        writeApi.WriteRecord(record: lp, bucket: bucket, org: org, precision: WritePrecision.S);

        _logger.LogInformation(
            "Registered temperature with id {Id} and value {Value}", 
            message.Id, message.Value
        );

        return Task.CompletedTask;
    }
}

