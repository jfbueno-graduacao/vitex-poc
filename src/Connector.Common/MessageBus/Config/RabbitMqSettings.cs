namespace Connector.Common.MessageBus.Config;

public sealed class RabbitMqSettings
{
    public const string ConfigurationKey = "RabbitMqConfig";
    public const string HostKey = $"{ConfigurationKey}:{nameof(Host)}";
    public const string UserKey = $"{ConfigurationKey}:{nameof(User)}";
    public const string PasswordKey = $"{ConfigurationKey}:{nameof(Password)}";
    
    public string Host { get; set; } = "";
    public string User { get; set; } = "";
    public string Password { get; set; } = "";
}
