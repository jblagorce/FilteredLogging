namespace MyOrg.DynamicLogging.AspNetCore.Serilog;

using System.Text;

using global::Serilog.Core;
using global::Serilog.Events;

using MyOrg.DynamicLogging.BroadCast;
using MyOrg.DynamicLogging.LogFilters;
using MyOrg.DynamicLogging.DebugContext;

public class NotifierSink : ILogEventSink
{
    public IDebugIfErrorContextProvider DebugIfErrorContextProvider { get; }
    public Func<INotificationPublisher> NotificationPublisherProvider { get; }

    public NotifierSink(IDebugIfErrorContextProvider debugIfErrorContextProvider, Func<INotificationPublisher> notificationPublisherProvider)
    {
        DebugIfErrorContextProvider = debugIfErrorContextProvider;
        NotificationPublisherProvider = notificationPublisherProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        if (DebugIfErrorContextProvider.Context != null)
        {
            NotificationPublisherProvider().Publish(
                new DynamicLogConfigItemSet()
                {
                    Expires = DateTime.UtcNow.AddMinutes(DebugIfErrorContextProvider.Context.Duration),
                    Items = DebugIfErrorContextProvider.Context.Fields.Select(f => new DynamicLogConfigItem { Field = f, Value = RenderProperty(logEvent, f) }).ToList()
                }
                );
        }
    }

    private string RenderProperty(LogEvent le, string propertyName)
    {
        StringBuilder sb = new StringBuilder();
        using var s = new StringWriter(sb);
        le.Properties[propertyName].Render(s, "l");
        return s.ToString();
    }
}

