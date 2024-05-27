namespace HE.Investments.TestsUtils.Extensions;

public static class UriExtensions
{
    private const char UrlPathSeparator = '/';

    public static string GetApplicationGuidFromUrl(this string uri)
    {
        return new Uri(uri).Segments[^2].Trim(UrlPathSeparator);
    }

    public static string GetSiteGuidFromUrl(this string uri)
    {
        var trueUri = new Uri(uri);
        var lastBefore = trueUri.Segments[^2].Trim(UrlPathSeparator);
        if (lastBefore == "site")
        {
            return trueUri.Segments[^1].Trim(UrlPathSeparator);
        }

        return lastBefore;
    }

    public static string GetProjectGuidFromUrl(this string uri)
    {
        return new Uri(uri).Segments[^2].Trim(UrlPathSeparator);
    }

    public static string GetProjectGuidFromRelativePath(this string uri)
    {
        var projectGuid = uri.Split(UrlPathSeparator)[^2];
        if (Guid.TryParse(projectGuid, out _))
        {
            return projectGuid;
        }

        return uri.Split("=")[^1];
    }

    public static string GetNestedGuidFromUrl(this string uri)
    {
        return new Uri(uri).Segments[^2].Trim(UrlPathSeparator);
    }
}
