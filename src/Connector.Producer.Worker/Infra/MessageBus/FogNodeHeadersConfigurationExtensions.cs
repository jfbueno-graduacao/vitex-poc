using Connector.Common.MessageBus.Contracts.Headers;
using Connector.Producer.Worker.Settings;
using MassTransit;

namespace Connector.Producer.Worker.Infra.MessageBus;

internal static class FogNodeHeadersConfigurationExtensions
{
    public static void UseFogNodeMetadataHeaders(
        this IPublishPipeConfigurator pipeConfigurator,
        IConfiguration configuration
    )
    {
        var fogNodeId = configuration.GetValue<string>(ConnectorSettings.FogNodeIdKey)
            ?? Guid.Empty.ToString();

        var fogNodeName = configuration.GetValue<string>(ConnectorSettings.FogNodeNameKey)
            ?? "<unspecified>";

        var coordinates = (
            latitude: configuration.GetValue<double>(ConnectorSettings.FogNodeLatitudeKey),
            longitude: configuration.GetValue<double>(ConnectorSettings.FogNodeLongitudeKey)
        );
        
        pipeConfigurator.UseExecute(publishContext =>
        {
            publishContext.Headers.Set(HeaderKeys.FogNodeId, fogNodeId);
            publishContext.Headers.Set(HeaderKeys.FogNodeName, fogNodeName);
            
            if (coordinates is not (0d, 0d))
            {
                publishContext.Headers.Set(HeaderKeys.FogNodeLatitude, coordinates.latitude);
                publishContext.Headers.Set(HeaderKeys.FogNodeLongitude, coordinates.longitude);
            }
        });
    }
}
