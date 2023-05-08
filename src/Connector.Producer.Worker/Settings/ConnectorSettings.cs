using System.ComponentModel.DataAnnotations;

namespace Connector.Producer.Worker.Settings;

internal sealed class ConnectorSettings
{
    public const string ConfigurationKey = "ConnectorConfig";
    public const string FogNodeInfoKey = $"{ConfigurationKey}:{nameof(FogNode)}";
    public const string FogNodeIdKey = $"{FogNodeInfoKey}:{nameof(FogNodeInfo.Id)}";
    public const string FogNodeNameKey = $"{FogNodeInfoKey}:{nameof(FogNodeInfo.Name)}";
    public const string FogNodeLatitudeKey = $"{FogNodeInfoKey}:{nameof(FogNodeInfo.Latitude)}";
    public const string FogNodeLongitudeKey = $"{FogNodeInfoKey}:{nameof(FogNodeInfo.Longitude)}";

    [Required]
    public FogNodeInfo FogNode { get; set; } = default!;

    // It's safe to define this initial value as "default!"
    // since this is a configuration class and therefore should be 
    // validated through the DataAnnotation above
}

internal sealed class FogNodeInfo
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}