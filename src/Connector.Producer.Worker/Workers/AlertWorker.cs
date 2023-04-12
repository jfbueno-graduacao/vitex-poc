using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Text;

namespace Connector.Producer.Worker.Workers;

internal class AlertWorker : BackgroundService
{
    private readonly ConnectorSettings _connectorSettings;
    private readonly HighTemperatureSharedState _highTemperatureSharedState;
    private readonly SmtpClient _smtpClient;
    private readonly MailAddress _senderMailAddress;

    public AlertWorker(
        IOptions<ConnectorSettings> connectorSettingsOptions,
        HighTemperatureSharedState highTemperatureSharedState, 
        SmtpClient smtpClient,
        MailAddress senderMailAddress
    )
    {
        _highTemperatureSharedState = highTemperatureSharedState;
        _smtpClient = smtpClient;
        _senderMailAddress = senderMailAddress;
        _connectorSettings = connectorSettingsOptions.Value;
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

                await _smtpClient.SendMailAsync(message, CancellationToken.None);

                _highTemperatureSharedState.ResetHistory();
            }

            await Task.Delay(5_000 * 60, stoppingToken);
        }
    }
}
