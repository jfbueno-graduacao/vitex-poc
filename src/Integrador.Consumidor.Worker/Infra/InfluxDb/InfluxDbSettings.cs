using System.ComponentModel.DataAnnotations;

namespace Integrador.Consumidor.Worker.Infra.InfluxDb;

internal sealed class InfluxDbSettings
{
    [Required]
    public string Host { get; set; } = "";

    [Required]
    public string Token { get; set; } = "";

    [Required]
    public string Organization { get; set; } = "";

    [Required]
    public string Bucket { get; set; } = "";
}
