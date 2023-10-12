namespace MyOrg.DynamicLogging.BroadCast.WebPubSub;

using System.Text.Json;
using System.Threading.Tasks;

using Azure.Messaging.WebPubSub;

using Microsoft.Extensions.Logging;

using MyOrg.DynamicLogging.BroadCast;
using MyOrg.DynamicLogging.LogFilters;

public class WebPubSubPublisher : INotificationPublisher
{
    public ILogger<WebPubSubPublisher> Logger { get; }
    public WebPubSubServiceClient ServiceClient { get; private set; }
    public WebPubSubSettings PubSubSettings { get; private set; }

    public WebPubSubPublisher(ILogger<WebPubSubPublisher> logger, WebPubSubSettings pubSubSettings)
    {
        Logger = logger;
        PubSubSettings = pubSubSettings;
        ServiceClient = new WebPubSubServiceClient(pubSubSettings.ConnectionString, pubSubSettings.HubName);
    }

    public async Task Publish(DynamicLogConfigItemSet itemSet)
    {
        await ServiceClient.SendToAllAsync(JsonSerializer.Serialize(itemSet));
    }

    public ValueTask DisposeAsync()
    {
        ServiceClient.CloseAllConnections();
        return ValueTask.CompletedTask;
    }
}
