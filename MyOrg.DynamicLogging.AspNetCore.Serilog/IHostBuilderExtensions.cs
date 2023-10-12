namespace MyOrg.DynamicLogging.AspNetCore.Serilog;

using global::Serilog;
using global::Serilog.Configuration;
using global::Serilog.Core;
using global::Serilog.Events;
using global::Serilog.Filters;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MyOrg.DynamicLogging.AspNetCore;
using MyOrg.DynamicLogging.BroadCast;
using MyOrg.DynamicLogging.DebugContext;
using MyOrg.DynamicLogging.LogFilters;

public static class IHostBuilderExtensions
{

    public static IHostBuilder SetupDynamicSerilog(this IHostBuilder hostBuilder,
                                                    Func<IServiceProvider, ILogEventEnricher[]> enrichers,
                                                    Func<IServiceProvider, LoggerSinkConfiguration, LoggerConfiguration> mainLogger,
                                                    Func<IServiceProvider, LoggerSinkConfiguration, LoggerConfiguration> debugLogger
                                                    )
    {
        return hostBuilder.UseSerilog((hbc, sp, loggerConf) =>
        {
            var dynamicLogConfigProvider = sp.GetRequiredService<DynamicLogConfigProvider>();

            loggerConf
                .Enrich.FromLogContext()
                .Enrich.With(enrichers(sp))
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithCorrelationId()
                .Enrich.WithClientIp()
                .Enrich.WithRequestHeader("User-Agent")
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.Logger(lc =>
                    lc
                    .MinimumLevel.Debug()
                    .Filter.ByIncludingOnly(FilterInEvent(dynamicLogConfigProvider))
                    .WriteTo.Map(_ => dynamicLogConfigProvider.Instance.GetHashCode().ToString(), (s, conf)
                    => debugLogger(sp, conf)
                    ))
                .WriteTo.Logger(lc =>
                    lc
                    .MinimumLevel.Information()
                    .WriteTo.Map(_ => "Default", (s, conf)
                    => mainLogger(sp, conf))
                )
                .WriteTo.Logger(lc =>
                    lc
                    .MinimumLevel.Verbose()
                    .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Verbose && Matching.WithProperty("SourceContext", typeof(ExceptionHandlingMiddleware).FullName)(le))
                    .WriteTo.NotifierSink(sp.GetRequiredService<IDebugIfErrorContextProvider>(), () => sp.GetRequiredService<INotificationPublisher>()));
        });
    }

    static Func<LogEvent, bool> FilterInEvent(DynamicLogConfigProvider configProvider)
    {
        return le => le.Level >= LogEventLevel.Debug
            &&
            configProvider.Instance.ItemSets.Any(i => i.Expires > DateTime.UtcNow && i.Items.All(item => Matching.WithProperty(item.Field, item.Value).Invoke(le)));
    }
}
