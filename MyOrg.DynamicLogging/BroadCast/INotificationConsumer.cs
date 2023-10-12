namespace MyOrg.DynamicLogging.BroadCast;

public interface INotificationConsumer : IAsyncDisposable
{
    Task Start();
    Task Stop();
}
