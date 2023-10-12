using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MyOrg.DynamicLogging.BroadCast;

namespace MyOrg.DynamicLogging.AspNetCore.BroadCast;

public class NotificationWorker : IHostedService, IDisposable
{
    public INotificationConsumer Consumer { get; }

    public ILogger<NotificationWorker> Logger { get; }

    public NotificationWorker(INotificationConsumer consumer,
        ILogger<NotificationWorker> logger)
    {
        Consumer = consumer;
        Logger = logger;
    }

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        await Consumer.Start();
        Logger.LogDebug("Starting the notification consumer");
    }

    public async Task StopAsync(CancellationToken stoppingToken)
    {
        Logger.LogDebug("Stopping the notification consumer");
        if (Consumer != null)
            await Consumer.Stop();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual async void Dispose(bool disposing)
    {
        if (disposing)
        {
            await Consumer.DisposeAsync().ConfigureAwait(false);
        }
    }
}
