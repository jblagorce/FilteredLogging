namespace MyOrg.DynamicLogging.BroadCast.Loopback;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

using MyOrg.DynamicLogging.LogFilters;

public class LoopbackConsumer : INotificationConsumer
{
    private ChannelReader<DynamicLogConfigItemSet> Reader { get; }
    public DynamicLogConfigProvider DynamicLogConfigProvider { get; }

    public LoopbackConsumer(ChannelReader<DynamicLogConfigItemSet> reader, DynamicLogConfigProvider dynamicLogConfigProvider)
    {
        Reader = reader;
        DynamicLogConfigProvider = dynamicLogConfigProvider;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public Task Start()
    {
        Task.Run(async () =>
        {
            while (await Reader.WaitToReadAsync())
            {
                while (Reader.TryRead(out var itemSet))
                {
                    DynamicLogConfigProvider.AddSet(itemSet);
                }
            }
        });
        return Task.CompletedTask;
    }

    public Task Stop()
    {
        return Task.CompletedTask;
    }
}

