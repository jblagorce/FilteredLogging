namespace MyOrg.DynamicLogging.AspNetCore;

using Microsoft.AspNetCore.Mvc.Filters;

using MyOrg.DynamicLogging.DebugContext;

public class DebugIfErrorAttribute : Attribute, IFilterMetadata
{
    public DebugIfErrorContext Context { get; }

    public DebugIfErrorAttribute(int duration, params string[] fields)
    {
        Context = new DebugIfErrorContext { Duration = duration, Fields = fields };
    }
}
