using System.ComponentModel.DataAnnotations;

namespace Connector.Producer.Worker;

internal sealed class ConnectorSettings
{
    public const string ConfigurationKey = "ConnectorConfig";
    public const string FogNodeInfoKey = $"{ConfigurationKey}:{nameof(FogNode)}";
    public const string FogNodeIdKey = $"{FogNodeInfoKey}:{nameof(FogNodeInfo.Id)}";
    public const string FogNodeNameKey = $"{FogNodeInfoKey}:{nameof(FogNodeInfo.Name)}";

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
}