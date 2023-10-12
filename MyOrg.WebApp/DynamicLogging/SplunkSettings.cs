namespace MyOrg.WebApp.DynamicLogging;

public class SplunkSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string MainToken { get; set; } = string.Empty;
    public string DebugToken { get; set; } = string.Empty;
}
