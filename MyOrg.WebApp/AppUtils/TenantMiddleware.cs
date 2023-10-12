﻿namespace MyOrg.WebApp.AppUtils;

/// <summary>
/// Extractes the tennant from the request and sets the tenant in the IContext
/// </summary>
public class TenantMiddleware
{
    public RequestDelegate Next { get; }
    public ILogger<TenantMiddleware> Logger { get; }
    public IContextProvider ContextProvider { get; }

    public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger, IContextProvider contextProvider)
    {
        Next = next;
        Logger = logger;
        ContextProvider = contextProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        var cegidContext = ContextProvider.GetContext();
        cegidContext.TenantName = context.Request.Host.Value.Split(".").First();
        await Next.Invoke(context);
    }
}
