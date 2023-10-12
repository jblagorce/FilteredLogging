namespace MyOrg.WebApp.DynamicLogging.Serilog;

using global::Serilog.Core;
using global::Serilog.Events;
using MyOrg.WebApp.AppUtils;

public class TenantNameEnricher : ILogEventEnricher
{
    public IContextProvider ContextProvider { get; init; }

    public TenantNameEnricher(IContextProvider contextProvider)
    {
        ContextProvider = contextProvider;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var prop = propertyFactory.CreateProperty("TenantName", ContextProvider.GetContext().TenantName);
        logEvent.AddOrUpdateProperty(prop);
    }
}