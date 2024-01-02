namespace HE.Investments.Common.WWW.Tests.Components;

public class Constants
{
#if DEBUG
    public const string? SkipTest = null;
#else
    public const string? SkipTest = "ViewComponents tests are failing on CI from time to time.";
#endif
}
