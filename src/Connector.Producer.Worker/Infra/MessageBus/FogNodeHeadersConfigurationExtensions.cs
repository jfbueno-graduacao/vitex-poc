using Connector.Common.MessageBus.Contracts.Headers;
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
        
        pipeConfigurator.UseExecute(publishContext =>
        {
            publishContext.Headers.Set(HeaderKeys.FogNodeId, fogNodeId);
            publishContext.Headers.Set(HeaderKeys.FogNodeName, fogNodeName);
        });
    }
}
