namespace MyOrg.DynamicLogging.AspNetCore;

using Microsoft.AspNetCore.Http;

using MyOrg.DynamicLogging.DebugContext;

public class DebugIfErrorContextProvider : IDebugIfErrorContextProvider
{
    public IHttpContextAccessor HttpContextAccessor { get; }

    public DebugIfErrorContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    public DebugIfErrorContext Context
    {
        get
        {
            {
                if (HttpContextAccessor.HttpContext == null || !HttpContextAccessor.HttpContext.Items.TryGetValue("DebugIfErrorContext", out var context))
                    return null;

                return (DebugIfErrorContext)context;
            }
        }
        set {
            if (HttpContextAccessor.HttpContext == null)
                return;
            HttpContextAccessor.HttpContext.Items["DebugIfErrorContext"] = value;
        }
    }
}
