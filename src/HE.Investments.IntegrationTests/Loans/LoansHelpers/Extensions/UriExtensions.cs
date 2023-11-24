namespace HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;

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
        var projectGuid = uri.Split('/')[^2];
        if (Guid.TryParse(projectGuid, out _))
        {
            return projectGuid;
        }

        return uri.Split("=").Last();
    }
}
