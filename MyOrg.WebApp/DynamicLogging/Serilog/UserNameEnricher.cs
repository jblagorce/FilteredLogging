namespace MyOrg.WebApp.DynamicLogging.Serilog;

using global::Serilog.Core;
using global::Serilog.Events;
using MyOrg.WebApp.AppUtils;

public class UserNameEnricher : ILogEventEnricher
{
    public IContextProvider ContextProvider { get; init; }

    public UserNameEnricher(IContextProvider contextProvider)
    {
        ContextProvider = contextProvider;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var prop = propertyFactory.CreateProperty("UserName", ContextProvider.GetContext().UserName);
        logEvent.AddOrUpdateProperty(prop);
    }
}