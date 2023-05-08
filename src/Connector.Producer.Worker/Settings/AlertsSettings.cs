using System.ComponentModel.DataAnnotations;

namespace Connector.Producer.Worker.Settings;

internal sealed class AlertsSettings
{
    [Required]
    public string SmtpHost { get; set; } = "";

    [Required]
    public int SmtpPort { get; set; }
}
