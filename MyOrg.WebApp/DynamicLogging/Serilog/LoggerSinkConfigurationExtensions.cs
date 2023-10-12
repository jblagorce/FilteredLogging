namespace MyOrg.WebApp.DynamicLogging.Serilog;

using global::Serilog.Configuration;
using global::Serilog;
using Microsoft.AspNetCore.SignalR;
using global::Serilog.Formatting.Compact;
using MyOrg.DynamicLogging.BroadCast;
using MyOrg.DynamicLogging.DebugContext;

public static class LoggerSinkConfigurationExtensions
{
    public static LoggerConfiguration SignalRSink<T>(
      this LoggerSinkConfiguration loggerConfiguration, Func<IHubContext<T>> hubProvider, bool debug) where T : Hub
    {
        return loggerConfiguration.Sink(new SignalRSink<T>(new CompactJsonFormatter(), hubProvider, debug));
    }
}
