using System.ComponentModel.DataAnnotations;

namespace Connector.Producer.Worker.Infra.MessageBus;

internal sealed class RabbitMqSettings
{
    public const string ConfigurationKey = "RabbitMqConfig";
    public const string UserKey = $"{ConfigurationKey}:{nameof(User)}";
    public const string PasswordKey = $"{ConfigurationKey}:{nameof(Password)}";
    
    public string User { get; set; } = "";
    public string Password { get; set; } = "";
}
