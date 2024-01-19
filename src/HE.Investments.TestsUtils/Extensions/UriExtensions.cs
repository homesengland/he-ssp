namespace HE.Investments.TestsUtils.Extensions;

public static class UriExtensions
{
    private const char UrlPathSeparator = '/';

    public static string GetApplicationGuidFromUrl(this string uri)
    {
        return new Uri(uri).Segments[^2].Trim(UrlPathSeparator);
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
