namespace MyOrg.DynamicLogging.BroadCast.Loopback;

using System.Threading.Channels;
using System.Threading.Tasks;

using MyOrg.DynamicLogging.LogFilters;

public class LoopbackPublisher : INotificationPublisher
{
    private ChannelWriter<DynamicLogConfigItemSet> Writer { get; }

    public LoopbackPublisher(ChannelWriter<DynamicLogConfigItemSet> writer)
    { 
        Writer = writer;
    }

    public ValueTask DisposeAsync()
    {
        Writer.Complete();
        return ValueTask.CompletedTask;
    }

    public async Task Publish(DynamicLogConfigItemSet itemSet)
    {
        await Writer.WriteAsync(itemSet);
    }
}

