namespace MyOrg.WebApp.DynamicLogging.Serilog; 

using global::Serilog.Core;
using global::Serilog.Events;

public class UserAgentEnricher : ILogEventEnricher
{
    public IHttpContextAccessor ContextAccessor { get; init; }

    public UserAgentEnricher(IHttpContextAccessor contextAccessor)
    {
        ContextAccessor = contextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {

        if (ContextAccessor==null || ContextAccessor.HttpContext==null || ContextAccessor.HttpContext.Request==null || !ContextAccessor.HttpContext.Request.Headers.ContainsKey("User-Agent"))
            return;
        var prop = propertyFactory.CreateProperty("User-Agent", ContextAccessor.HttpContext.Request.Headers["User-Agent"].FirstOrDefault());
        logEvent.AddOrUpdateProperty(prop);
    }
}