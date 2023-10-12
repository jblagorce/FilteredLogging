namespace MyOrg.DynamicLogging.LogFilters;

/// <summary>
/// A set of combined filtering rules (to be combined with logical AND) that will be applied to the log events.
/// </summary>
public class DynamicLogConfigItemSet
{
    public List<DynamicLogConfigItem> Items { get; init; } = new List<DynamicLogConfigItem>();
    public DateTime Expires { get; init; }

    public override int GetHashCode()
    {
        return Items.Aggregate(0, (acc, item) => acc ^ item.GetHashCode());
    }
}
