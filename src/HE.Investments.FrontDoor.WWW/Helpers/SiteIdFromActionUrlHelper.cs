using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.WWW.Helpers;

public static class SiteIdFromActionUrlHelper
{
    public static string Get(string? actionUrl)
    {
        if (actionUrl.IsNotProvided())
        {
            return string.Empty;
        }

        var segments = actionUrl!.Split('/');
        var siteId = segments[^2];
        return siteId;
    }
}
