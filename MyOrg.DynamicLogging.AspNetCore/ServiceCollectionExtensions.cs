namespace MyOrg.DynamicLogging.AspNetCore
{
    using Microsoft.Extensions.DependencyInjection;

    using MyOrg.DynamicLogging.AspNetCore.BroadCast;
    using MyOrg.DynamicLogging.DebugContext;
    using MyOrg.DynamicLogging.LogFilters;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDynamicLoggingServices(this IServiceCollection services)
        {
            services.AddSingleton<DynamicLogConfigProvider>();
            services.AddHostedService<NotificationWorker>();
            services.AddHttpContextAccessor();
            services.AddSingleton<IDebugIfErrorContextProvider, DebugIfErrorContextProvider>();

            services.AddHttpContextAccessor();
            services.ConfigureOptions<MvcOptionsConfiguration>();
            return services;
        }
    }
}
