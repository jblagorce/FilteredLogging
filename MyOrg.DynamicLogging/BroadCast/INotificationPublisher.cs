using MyOrg.DynamicLogging.LogFilters;

namespace MyOrg.DynamicLogging.BroadCast;

public interface INotificationPublisher : IAsyncDisposable
{
    Task Publish(DynamicLogConfigItemSet itemSet);
}
