namespace MyOrg.DynamicLogging.AspNetCore.Serilog;

using global::Serilog.Configuration;
using global::Serilog;
using MyOrg.DynamicLogging.BroadCast;
using MyOrg.DynamicLogging.DebugContext;

public static class LoggerSinkConfigurationExtensions
{
    public static LoggerConfiguration NotifierSink(
      this LoggerSinkConfiguration loggerConfiguration,
      IDebugIfErrorContextProvider debugIfErrorContextProvider,
      Func<INotificationPublisher> notificationPublisherProvider)
    {
        return loggerConfiguration.Sink(new NotifierSink(debugIfErrorContextProvider, notificationPublisherProvider));
    }

}
