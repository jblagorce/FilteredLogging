namespace MyOrg.DynamicLogging.AspNetCore;

using Microsoft.AspNetCore.Mvc.Filters;

using MyOrg.DynamicLogging.DebugContext;

public class DebugIfErrorFilter : IActionFilter
{
    public IDebugIfErrorContextProvider DebugIfErrorContextProvider { get; }

    public DebugIfErrorFilter(IDebugIfErrorContextProvider context)
    {
        DebugIfErrorContextProvider = context;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var debugIfErrorAttribute = context.ActionDescriptor.FilterDescriptors
            .Select(x => x.Filter).OfType<DebugIfErrorAttribute>().FirstOrDefault();

        if (debugIfErrorAttribute != null)
        {
            DebugIfErrorContextProvider.Context = debugIfErrorAttribute.Context;
        }
    }
}
