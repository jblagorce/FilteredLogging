namespace MyOrg.WebApp.Controllers;

using Microsoft.AspNetCore.Mvc;

using MyOrg.DynamicLogging.AspNetCore;
using MyOrg.WebApp.Models.Pythagoras;

[Route("[controller]")]
[ApiController]
public class PythagorasController : ControllerBase
{
    public ILogger<PythagorasController> Logger { get; }
    public IServiceProvider ServiceProvider { get; }

    public PythagorasController(ILogger<PythagorasController> logger, IServiceProvider sp)
    {
        Logger = logger;
        ServiceProvider = sp;
    }

    [DebugIfError(5, "RequestPath", "TenantName", "UserName")]
    [HttpPost(Name = "PostPythagoras")]
    public async Task<ActionResult<int>> Post(PythagorasModel model)
    {
        await Task.Delay(1);
        Logger.LogDebug("Pythagoras invoked with model {Side1} {Side2}", model.Side1, model.Side2);
        var result = Convert.ToInt32(Math.Sqrt(model.Side1 * model.Side1 + model.Side2 * model.Side2));
        return result;
    }
}
