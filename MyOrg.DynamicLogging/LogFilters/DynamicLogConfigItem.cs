namespace MyOrg.DynamicLogging.LogFilters;

/// <summary>
/// A filtering rule (Field=Value) that will be applied to the log events.
/// </summary>
public class DynamicLogConfigItem
{
    public string Field { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;

    public override int GetHashCode()
    {
        return Field.GetHashCode() ^ Value.GetHashCode();
    }
}
