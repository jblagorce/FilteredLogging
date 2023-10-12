namespace MyOrg.WebApp.DynamicLogging.Serilog; 

using System.Text;

using Microsoft.AspNetCore.SignalR;

using global::Serilog.Core;
using global::Serilog.Events;
using global::Serilog.Formatting;

public class SignalRSink<T> : ILogEventSink where T : Hub
{
    private readonly ITextFormatter _formatProvider;
    private readonly Func<IHubContext<T>> _hubProvider;
    private readonly bool _debug = false;

    public SignalRSink(ITextFormatter formatProvider, Func<IHubContext<T>> hubAccessor, bool debug)
    {
        _formatProvider = formatProvider;
        _hubProvider = hubAccessor;
        _debug = debug;
    }

    public void Emit(LogEvent logEvent)
    {
        logEvent.AddOrUpdateProperty(new LogEventProperty("Level", new ScalarValue(logEvent.Level.ToString())));
        StringBuilder sb = new StringBuilder();
        using var s = new StringWriter(sb);
        //logEvent.Properties["Timestamp"].Render(s);
        _formatProvider.Format(logEvent, s);
        _hubProvider().Clients.All.SendAsync(_debug ? "ReceiveDebugMessage" : "ReceiveMainMessage", s.ToString());
    }
}
