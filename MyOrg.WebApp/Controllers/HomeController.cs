namespace MyOrg.WebApp.Controllers;

using Microsoft.AspNetCore.Mvc;

using MyOrg.WebApp.AppUtils;
using MyOrg.WebApp.Models.Home;

public class HomeController : Controller
{
    public IContextProvider ContextProvider { get; }

    public ILogger<HomeController> Logger { get; }

    public HomeController(IContextProvider contextProvider, ILogger<HomeController> logger)
    {
        ContextProvider = contextProvider;
        Logger = logger;
    }


    [HttpGet]
    public IActionResult Index()
    {
        return View(new IndexViewModel { UserName = Request.Cookies["UserName"]??"UserName", TenantName = ContextProvider.GetContext().TenantName});
    }

    [HttpPost]
    public IActionResult UserName([FromForm]string userName)
    { 
        Response.Cookies.Append("UserName", userName);
        return RedirectToAction("Index");
    }
}
