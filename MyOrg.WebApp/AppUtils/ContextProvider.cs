namespace MyOrg.WebApp.AppUtils;

public class ContextProvider : IContextProvider
{
    private static AsyncLocal<Context> Context = new AsyncLocal<Context>();

    public IContext GetContext()
    {
        return Context.Value ??= new Context();
    }

}

public class Context : IContext
{
    public string TenantName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}
