namespace HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Extensions;

public static class UriExtensions
{
    public static string GetApplicationGuidFromUrl(this string uri)
    {
        return new Uri(uri).Segments[^2][..^1];
    }

    public static string GetProjectGuidFromUrl(this string uri)
    {
        return new Uri(uri).Segments[^2][..^1];
    }

    public static string GetProjectGuidFromRelativePath(this string uri)
    {
        return uri.Split('/')[^2];
    }
}
