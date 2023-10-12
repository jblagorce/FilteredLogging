namespace MyOrg.DynamicLogging.LogFilters;
public class DynamicLogConfig
{
    private readonly int _hashCode;
    public int HashCode => _hashCode;
    public int FromHashCode { get; }

    public IReadOnlyCollection<DynamicLogConfigItemSet> ItemSets { get; private set; }

    public DynamicLogConfig()
    {
        ItemSets = new List<DynamicLogConfigItemSet>();
    }

    public DynamicLogConfig(DynamicLogConfig from, DynamicLogConfigItemSet itemSet)
    {
        FromHashCode = from.HashCode;
        ItemSets = from.ItemSets.Where(itemSet => itemSet.Expires > DateTime.UtcNow).Append(itemSet).ToList();
        _hashCode = ItemSets.Aggregate(0, (acc, item) => acc ^ item.GetHashCode());
    }
}
