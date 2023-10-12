namespace MyOrg.DynamicLogging.BroadCast.Redis;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MyOrg.DynamicLogging.LogFilters;
using MyOrg.WebApp.DynamicLogging.Redis;

using StackExchange.Redis;

public class RedisPublisher : INotificationPublisher
{
    public ILogger<RedisPublisher> Logger { get; }
    public RedisSettings RedisSettings { get; }
    public ConnectionMultiplexer Redis { get; private set; }

    public RedisPublisher(ILogger<RedisPublisher> logger, RedisSettings redisSettings)
    {
        Logger = logger;
        Redis = ConnectionMultiplexer.Connect(redisSettings.ConnectionString);
        RedisSettings = redisSettings;
    }

    public async Task Publish(DynamicLogConfigItemSet itemSet)
    {
        IDatabase db = Redis.GetDatabase();
        await db.PublishAsync(RedisSettings.ChannelName, JsonSerializer.Serialize(itemSet));
    }

    public async ValueTask DisposeAsync()
    {
        await Redis.DisposeAsync();
    }
}
