namespace MyOrg.WebApp.AppUtils
{
    using MyOrg.WebApp.DynamicLogging.Serilog;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IContextProvider, ContextProvider>();
            services.AddSingleton<TenantNameEnricher>();
            services.AddSingleton<UserAgentEnricher>();
            services.AddSingleton<UserNameEnricher>();
            return services;
        }
    }
}
