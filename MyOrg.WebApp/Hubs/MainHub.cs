namespace MyOrg.WebApp.Hubs;

using Microsoft.AspNetCore.SignalR;

public class MainHub : Hub
{
    public async Task SendMainMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMainMessage", message);
    }
}
