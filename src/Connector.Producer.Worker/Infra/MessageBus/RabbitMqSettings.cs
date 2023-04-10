using System.ComponentModel.DataAnnotations;

namespace Connector.Produtor.Worker.Infra.MessageBus;

internal sealed class RabbitMqSettings
{
    public const string ConfigurationKey = "RabbitMqConfig";
    public const string UserKey = $"{ConfigurationKey}:{nameof(User)}";
    public const string PasswordKey = $"{ConfigurationKey}:{nameof(Password)}";

    [Required]
    public string User { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}
