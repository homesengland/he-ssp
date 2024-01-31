namespace HE.Investments.Loans.IntegrationTests.Loans;

public static class LoansConfig
{
    // Make it null when you want to run tests locally
#if DEBUG
    public const string? SkipTest = null;
#else
    public const string? SkipTest = "Waits for DevOps configuration - #76791";
#endif
}
