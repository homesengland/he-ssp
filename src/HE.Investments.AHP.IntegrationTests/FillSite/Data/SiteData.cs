using System.Globalization;

namespace HE.Investments.AHP.IntegrationTests.FillSite.Data;

public class SiteData
{
    public SiteData()
    {
    }

    public string SiteId { get; private set; }

    public string SiteName { get; private set; }

    public string GenerateSiteName()
    {
        SiteName = $"IT_Site_{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
        return SiteName;
    }

    public void SetSiteId(string siteId)
    {
        SiteId = siteId;
    }
}
