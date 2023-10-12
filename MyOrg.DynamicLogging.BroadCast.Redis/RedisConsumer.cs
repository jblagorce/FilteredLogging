namespace MyOrg.WebApp.DynamicLogging.Redis;

using System.Text.Json;
using MyOrg.DynamicLogging.BroadCast;

using StackExchange.Redis;
using MyOrg.DynamicLogging.LogFilters;
using Microsoft.Extensions.Logging;

public class RedisConsumer : INotificationConsumer
{
    public DynamicLogConfigProvider DynamicLogConfigProvider { get; set; } = null!;
    public ILogger<RedisConsumer> Logger { get; }
    public RedisSettings RedisSettings { get; }

    public ConnectionMultiplexer? ConnectionMultiplexer { get; private set; }

    public ISubscriber? Subscriber { get; private set; }

    public RedisConsumer(ILogger<RedisConsumer> logger, RedisSettings redisSettings, DynamicLogConfigProvider dynamicLogConfigProvider)
    {
        DynamicLogConfigProvider = dynamicLogConfigProvider;
        Logger = logger;
        RedisSettings = redisSettings;
    }

    public async Task Start()
    {
        ConnectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(RedisSettings.ConnectionString);
        Subscriber = ConnectionMultiplexer.GetSubscriber();

        await Subscriber.SubscribeAsync(RedisSettings.ChannelName, (channel, message) =>
        {
            try
            {
                DynamicLogConfigItemSet itemSet = JsonSerializer.Deserialize<DynamicLogConfigItemSet>(message);
                if (itemSet != null)
                    DynamicLogConfigProvider.AddSet(itemSet);
                else
                    Logger.LogWarning("Failed deserializing message as DynamicLogConfigItemSet : message is null");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed deserializing message", message);
            }
        });
    }

    public async Task Stop()
    {
        if (Subscriber == null)
            return;
        await Subscriber.UnsubscribeAsync(RedisSettings.ChannelName);
    }

    public async ValueTask DisposeAsync()
    {
        if (ConnectionMultiplexer != null)
            await ConnectionMultiplexer.DisposeAsync();
    }
}
