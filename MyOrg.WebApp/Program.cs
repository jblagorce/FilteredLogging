using Microsoft.AspNetCore.SignalR;

using MyOrg.WebApp.AppUtils;
using MyOrg.DynamicLogging.AspNetCore;
using MyOrg.DynamicLogging.AspNetCore.Serilog;
using MyOrg.WebApp.DynamicLogging.Redis;
using MyOrg.WebApp.DynamicLogging;
using MyOrg.WebApp.DynamicLogging.Serilog;
using MyOrg.WebApp.DynamicLogging.WebPubSub;
using MyOrg.WebApp.Hubs;

using Serilog;
using MyOrg.DynamicLogging.BroadCast.Loopback;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddSingleton<SplunkSettings>(sp => sp.GetRequiredService<IConfiguration>().GetRequiredSection("Splunk").Get<SplunkSettings>());

builder.Services.ConfigureServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureDynamicLoggingServices();
builder.Services.AddMvc();

builder.Host.SetupDynamicSerilog(
    sp => new Serilog.Core.ILogEventEnricher[] { sp.GetRequiredService<TenantNameEnricher>(), sp.GetRequiredService<UserNameEnricher>() },
    (sp, conf) => conf.SignalRSink(() => sp.GetRequiredService<IHubContext<MainHub>>(), false),
    (sp, conf) => conf.SignalRSink(() => sp.GetRequiredService<IHubContext<MainHub>>(), true))
    .WithLoopbackNotifications();
    // .WithWebPubSubNotifications(); // need to setup a Web PubSub
    // .WithRedisNotifications();   // need to setup a Redis

    // Splunk with Redis notifications (need to setup a Splunk)
    // builder.Host.SetupDynamicSerilogToSplunk().WithRedisNotifications();

    // File with Redis notifications
    //builder.Host.SetupDynamicSerilogToFile().WithRedisNotifications();

var app = builder.Build();

app.UseMiddleware<TenantMiddleware>();
app.UseMiddleware<UserNameMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapHub<MainHub>("/mainHub");

app.Run();
