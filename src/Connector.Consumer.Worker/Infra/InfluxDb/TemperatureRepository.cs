using Connector.Consumer.Worker.Infra.Model;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Connector.Consumer.Worker.Infra.InfluxDb;

internal sealed class TemperatureRepository
{
    private readonly ILogger<TemperatureRepository> _logger;
    private readonly InfluxDbSettings _influxDbSettings;

    public TemperatureRepository(
        ILogger<TemperatureRepository> logger,
        IOptions<InfluxDbSettings> influxDbSettingsOptions
    )
    {
        _logger = logger;
        _influxDbSettings = influxDbSettingsOptions.Value;
    }

    public void AddRange(
        FogNodeMetadata fogNodeMetadata,
        IReadOnlyCollection<Temperature> temperaturesToInsert
    )
    {
        using var client = new InfluxDBClient(
            url: _influxDbSettings.Host,
            token: _influxDbSettings.Token
        );

        Debug.Assert(temperaturesToInsert.Any());

        var records = temperaturesToInsert.Select(t => 
            PointData.Measurement("temperature")
                .Tag("personId", t.PersonId.ToString())
                .Tag("fogNodeId", fogNodeMetadata.Id.ToString())
                .Tag("fogNodeName", fogNodeMetadata.Name)
                .Field("value", t.Value)
                .Timestamp(t.Timestamp, WritePrecision.S)
                .ToLineProtocol()
        )
        .ToList();

        Debug.Assert(records.Any(), "Projection didn't generate any points");
        
        using var writeApi = client.GetWriteApi();
        
        _logger.LogDebug(
            "Line protocol message being sent: {LineProtocol}",
            string.Join(Environment.NewLine, records)
        );

        writeApi.WriteRecords(
            records: records,
            bucket: _influxDbSettings.Bucket,
            org: _influxDbSettings.Organization,
            precision: WritePrecision.S
        );
    }
}