namespace MyOrg.DynamicLogging.DebugContext;

public interface IDebugIfErrorContextProvider
{
    DebugIfErrorContext Context { get; set; }
}
