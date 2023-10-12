namespace MyOrg.DynamicLogging.AspNetCore;

using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using MyOrg.DynamicLogging.DebugContext;

using Newtonsoft.Json;

public class ExceptionHandlingMiddleware
{
    public RequestDelegate requestDelegate;
    public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
    {
        this.requestDelegate = requestDelegate;
    }
    public async Task Invoke(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger, IDebugIfErrorContextProvider debugIfErrorContextProvider)
    {
        try
        {
            await requestDelegate(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred {ErrorMessage}", ex.Message);
            logger.LogTrace("Switch to debug mode");
            await HandleException(context, ex);
        }
    }
    private static Task HandleException(HttpContext context, Exception ex)
    {
        var errorMessage = JsonConvert.SerializeObject(new { ex.Message, Code = "ErrorCode" });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(errorMessage);
    }
}