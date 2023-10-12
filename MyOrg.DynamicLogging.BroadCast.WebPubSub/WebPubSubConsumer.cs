namespace MyOrg.DynamicLogging.BroadCast.WebPubSub;

using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;

using Azure.Messaging.WebPubSub;

using Microsoft.Extensions.Logging;

using MyOrg.DynamicLogging.BroadCast;
using MyOrg.DynamicLogging.LogFilters;

using Websocket.Client;

public class WebPubSubConsumer : INotificationConsumer
{
    public ILogger<WebPubSubConsumer> Logger { get; }
    public WebPubSubSettings PubSubSettings { get; }
    public WebsocketClient? WebsocketClient { get; private set; }
    public WebPubSubServiceClient? ServiceClient { get; private set; }
    public DynamicLogConfigProvider DynamicLogConfigProvider { get; private set; }

    public WebPubSubConsumer(ILogger<WebPubSubConsumer> logger, WebPubSubSettings settings, DynamicLogConfigProvider dynamicLogConfigProvider)
    {
        DynamicLogConfigProvider = dynamicLogConfigProvider;
        Logger = logger;
        PubSubSettings = settings;
    }

    public async Task Start()
    {
        ServiceClient = new WebPubSubServiceClient(PubSubSettings.ConnectionString, PubSubSettings.HubName);
        var url = ServiceClient.GetClientAccessUri();
        WebsocketClient = new WebsocketClient(url);
        WebsocketClient.ReconnectTimeout = null;
        WebsocketClient.MessageReceived.Subscribe(msg =>
        {
            if (msg.MessageType == WebSocketMessageType.Text)
            {
                var itemSet = JsonSerializer.Deserialize<DynamicLogConfigItemSet>(msg.Text);
                if (itemSet != null)
                    DynamicLogConfigProvider.AddSet(itemSet);
            }
        });
        await WebsocketClient.Start();
    }

    public async Task Stop()
    {
        if (WebsocketClient != null)
            await WebsocketClient.Stop(WebSocketCloseStatus.NormalClosure, WebSocketCloseStatus.NormalClosure.ToString());
    }

    public ValueTask DisposeAsync()
    {
        if (WebsocketClient != null)
            WebsocketClient.Dispose();
        return ValueTask.CompletedTask;
    }
}
