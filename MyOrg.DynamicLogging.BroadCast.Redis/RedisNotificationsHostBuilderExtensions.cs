namespace MyOrg.WebApp.DynamicLogging.Redis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MyOrg.DynamicLogging.BroadCast;
using MyOrg.DynamicLogging.BroadCast.Redis;

public static class RedisNotificationsHostBuilderExtensions
{
    public static IHostBuilder WithRedisNotifications(this IHostBuilder builder)
    {
        return builder.ConfigureServices((context, services) =>
        {
            services.AddSingleton(sp => sp.GetRequiredService<IConfiguration>().GetRequiredSection("Redis").Get<RedisSettings>());
            services.AddTransient<INotificationConsumer, RedisConsumer>();
            services.AddSingleton<INotificationPublisher, RedisPublisher>();
        });
    }
}
