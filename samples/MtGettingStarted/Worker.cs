using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace MtGettingStarted;

public class Worker : BackgroundService
{
    private readonly IBus _bus;

    public Worker(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var counter = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            await _bus.Publish(new HelloMessage
            {
                Name = $"Jéf {++counter}"
            }, stoppingToken);
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}
