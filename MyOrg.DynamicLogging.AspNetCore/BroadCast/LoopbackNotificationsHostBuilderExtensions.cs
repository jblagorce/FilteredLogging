namespace MyOrg.DynamicLogging.BroadCast.Loopback;

using System.Threading.Channels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MyOrg.DynamicLogging.LogFilters;

public static class LoopbackNotificationsHostBuilderExtensions
{
    public static IHostBuilder WithLoopbackNotifications(this IHostBuilder builder)
    {
        return builder.ConfigureServices((context, services) =>
        {
            var channel = Channel.CreateUnbounded<DynamicLogConfigItemSet>(new UnboundedChannelOptions { SingleWriter = true });
            services.AddTransient<ChannelReader<DynamicLogConfigItemSet>>(sp => channel.Reader);
            services.AddSingleton<ChannelWriter<DynamicLogConfigItemSet>>(sp => channel.Writer);
            services.AddTransient<INotificationConsumer, LoopbackConsumer>();
            services.AddSingleton<INotificationPublisher, LoopbackPublisher>();

        });
    }
}
