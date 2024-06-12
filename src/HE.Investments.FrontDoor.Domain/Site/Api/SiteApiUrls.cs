using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Site.Api;

internal static class SiteApiUrls
{
    public const string SaveSite = "upsertProjectSite";

    public static string GetSite(string siteId) => $"getProjectSite/{siteId.ToGuidAsString()}";
}
