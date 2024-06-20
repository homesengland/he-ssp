using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Site.Storage.Api;

internal static class SiteApiUrls
{
    public static string Site(string projectId, string? siteId = null)
    {
        var url = $"projects/{projectId.ToGuidAsString()}/sites";
        return string.IsNullOrEmpty(siteId) ? url : $"{url}/{siteId.ToGuidAsString()}";
    }

    public static string RemoveSite(string projectId, string siteId) =>
        $"projects/{projectId.ToGuidAsString()}/sites/{siteId.ToGuidAsString()}";
}
