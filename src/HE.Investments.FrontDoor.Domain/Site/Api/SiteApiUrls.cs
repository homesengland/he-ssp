using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Site.Api;

internal static class SiteApiUrls
{
    public const string SaveSite = "upsertProjectSite";

    public const string RemoveSite = "DeactivateProjectSite";

    public static string GetSite(string siteId) => $"getProjectSite/{siteId.ToGuidAsString()}";

    public static string GetSites(string projectId) => $"getProjectSites/{projectId.ToGuidAsString()}";
}
