namespace MyOrg.DynamicLogging.LogFilters;

public class DynamicLogConfigProvider
{
    public DynamicLogConfig Instance { get; private set; } = new DynamicLogConfig();
    public object locker = new object();

    public void AddSet(DynamicLogConfigItemSet itemSet)
    {
        var newConfig = new DynamicLogConfig(Instance, itemSet);
        var success = false;
        while (!success)
        {
            while (newConfig.FromHashCode != Instance.HashCode)
            {
                newConfig = new DynamicLogConfig(Instance, itemSet);
            }
            lock (locker)
            {
                if (newConfig.FromHashCode == Instance.HashCode)
                {
                    Instance = newConfig;
                    success = true;
                }
            }
        }

    }
}
