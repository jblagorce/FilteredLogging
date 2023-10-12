namespace MyOrg.WebApp.DynamicLogging.Serilog;

using global::Serilog;
using global::Serilog.Core;
using global::Serilog.Formatting.Compact;

using MyOrg.DynamicLogging.AspNetCore.Serilog;

public static class IHostBuilderExtensions
{
    public static IHostBuilder SetupDynamicSerilogToFile(this IHostBuilder hostBuilder)
    {
        return hostBuilder.SetupDynamicSerilog(
                sp => new ILogEventEnricher[] { sp.GetRequiredService<TenantNameEnricher>(), sp.GetRequiredService<UserNameEnricher>() },
                (sp, conf) => conf.File(new CompactJsonFormatter(), "main.txt"),
                (sp, conf) => conf.File(new CompactJsonFormatter(), "debug.txt"));
    }

    public static IHostBuilder SetupDynamicSerilogToSplunk(this IHostBuilder hostBuilder)
    {
        return hostBuilder.SetupDynamicSerilog(
                sp => new ILogEventEnricher[] { sp.GetRequiredService<UserNameEnricher>(), sp.GetRequiredService<TenantNameEnricher>()},
                (sp, conf) => conf.EventCollector(sp.GetRequiredService<SplunkSettings>().ConnectionString, sp.GetRequiredService<SplunkSettings>().MainToken),
                (sp, conf) => conf.EventCollector(sp.GetRequiredService<SplunkSettings>().ConnectionString, sp.GetRequiredService<SplunkSettings>().DebugToken));
    }
}
