using Connector.Producer.Worker.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Text;

namespace Connector.Producer.Worker.Workers;

internal class AlertWorker : BackgroundService
{
    private readonly ConnectorSettings _connectorSettings;
    private readonly AlertsSettings _alertsSettings;
    private readonly HighTemperatureSharedState _highTemperatureSharedState;
    private readonly MailAddress _senderMailAddress;
    private readonly ILogger<AlertWorker> _logger;

    public AlertWorker(
        IOptions<ConnectorSettings> connectorSettingsOptions,
        IOptions<AlertsSettings> alertsSettingsOptions,
        HighTemperatureSharedState highTemperatureSharedState,
        MailAddress senderMailAddress, 
        ILogger<AlertWorker> logger
    )
    {
        _highTemperatureSharedState = highTemperatureSharedState;
        _senderMailAddress = senderMailAddress;
        _logger = logger;
        _connectorSettings = connectorSettingsOptions.Value;
        _alertsSettings = alertsSettingsOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var countOfPeopleWithHighTemperature = 
                _highTemperatureSharedState.GetRepeatedValues(10).Length;

            if (countOfPeopleWithHighTemperature > 0)
            {
                var message = new MailMessage(_senderMailAddress, new MailAddress("responsaveis@hospital"))
                {
                    Body = $"{countOfPeopleWithHighTemperature} pessoas têm apresentado registros " +
                           $"de temperatura alta nos últimos minutos em {_connectorSettings.FogNode.Name}",
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = false,
                    Priority = MailPriority.High,
                    Sender = _senderMailAddress,
                    Subject = "Alerta de casos de temperatura alta",
                    SubjectEncoding = Encoding.UTF8
                };

                using var smtpClient 
                    = new SmtpClient(_alertsSettings.SmtpHost, _alertsSettings.SmtpPort);

                try
                {
                    await smtpClient.SendMailAsync(message, CancellationToken.None);
                }
                catch (SmtpException ex) 
                {
                    // If there's a failure sending an alert, we should only log it 
                    // to do some troubleshooting in that component
                    // Since we're trying this as a prototype and our SMTP server doesn't
                    // support much load, we'll just ignore it for now
                    _logger.LogError(
                        ex, 
                        "Problem while sending alert to {SmtpHost}:{SmtpPort}",
                        _alertsSettings.SmtpHost, 
                        _alertsSettings.SmtpPort
                    );
                }

                _highTemperatureSharedState.ResetHistory();
            }

            await Task.Delay(120_000, stoppingToken);
        }
    }
}
