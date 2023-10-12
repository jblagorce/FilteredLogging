namespace MyOrg.WebApp.AppUtils;

/// <summary>
/// Extractes the tenant from the request and sets the tenant in the IContext
/// </summary>
public class UserNameMiddleware
{
    public RequestDelegate Next { get; }
    public ILogger<TenantMiddleware> Logger { get; }
    public IContextProvider ContextProvider { get; }

    public UserNameMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger, IContextProvider contextProvider)
    {
        Next = next;
        Logger = logger;
        ContextProvider = contextProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        var cegidContext = ContextProvider.GetContext();
        cegidContext.UserName = context.Request.Cookies["UserName"];
        await Next.Invoke(context);
    }
}
