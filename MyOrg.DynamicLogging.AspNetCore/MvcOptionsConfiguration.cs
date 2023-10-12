namespace MyOrg.DynamicLogging.AspNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

public class MvcOptionsConfiguration : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        options.Filters.Add<DebugIfErrorFilter>();
    }
}
