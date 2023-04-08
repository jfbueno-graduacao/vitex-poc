using Integrador.Common.MessageBus.Contracts;
using Integrador.Produtor.Worker.Infra.Database;
using MassTransit;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace Integrador.Produtor.Worker.Infra.MessageBus;

internal class TemperatureBusProxy
{
    private readonly IBus _messageBus;

    public TemperatureBusProxy(IBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Publish(IEnumerable<Temperature> temperaturesToSend, CancellationToken cancellationToken)
    {
        // TODO: Implementar envio em lote direto
        // Qual o limite de tamanho de mensagens do broker?

        foreach (Temperature temperature in temperaturesToSend)
        {
            await _messageBus.Publish(new TemperatureToInsertMessage
            {
                Id = temperature.Id,
                PersonId = temperature.PersonId,
                Timestamp = temperature.Timestamp,
                Value = temperature.Value
            }, cancellationToken);
        }
    }

    private static byte[] ObjectToByteArray(object obj)
        => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));

    private static async Task CompressThisShit(byte[] data, CancellationToken stoppingToken)
    {
        using var compressedStream = new MemoryStream();
        await using var compressor = new GZipStream(compressedStream, CompressionLevel.SmallestSize);

        await compressor.WriteAsync(data, 0, data.Length, stoppingToken);
        await compressor.FlushAsync(stoppingToken);
        var compressedData = compressedStream.ToArray();

        //using MemoryStream originalStream = new MemoryStream(data, false);
        //await originalStream.CopyToAsync(compressor, stoppingToken);

        //using var ms = new MemoryStream();
        //await compressedStream.CopyToAsync(ms, stoppingToken);
        //var compressedByteArray = ms.ToArray();
    }
}
