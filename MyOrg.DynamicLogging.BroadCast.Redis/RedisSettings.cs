namespace MyOrg.WebApp.DynamicLogging.Redis;

public class RedisSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ChannelName { get; set; } = string.Empty;
}
