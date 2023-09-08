namespace HE.InvestmentLoans.IntegrationTests.LoansHelpers.Extensions;

public static class UriExtensions
{
    public static string GetApplicationGuidFromUrl(this string uri)
    {
        return new Uri(uri).Segments[^2][..^1];
    }
}
