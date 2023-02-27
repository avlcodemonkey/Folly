namespace Folly;

public static class PetchConstants
{
    public const string PetchHeader = "X-PETCH";
    public const string PetchVersion = "X-PETCH-VERSION";
    public static readonly string PetchVersionValue = Guid.NewGuid().ToString("N");
}
