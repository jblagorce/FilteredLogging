namespace MyOrg.WebApp.DynamicLogging.WebPubSub;

using MyOrg.DynamicLogging.BroadCast.WebPubSub;
using MyOrg.DynamicLogging.BroadCast;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

public static class WebPubSubNotificationsHostBuilderExtensions
{
    public static IHostBuilder WithWebPubSubNotifications(this IHostBuilder builder)
    {
        return builder.ConfigureServices((context, services) =>
        {
            services.AddSingleton(sp => sp.GetRequiredService<IConfiguration>().GetRequiredSection("AzureWebPubSub").Get<WebPubSubSettings>());
            services.AddTransient<INotificationConsumer, WebPubSubConsumer>();
            services.AddSingleton<INotificationPublisher, WebPubSubPublisher>();
        });
    }
}
