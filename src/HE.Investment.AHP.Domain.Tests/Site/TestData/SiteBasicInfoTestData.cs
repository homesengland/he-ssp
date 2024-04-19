using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Site.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Site.TestData;

public static class SiteBasicInfoTestData
{
    public static SiteBasicInfo DefaultSite(SiteId siteId) => new(
        siteId,
        new SiteName("Site name"),
        null,
        null,
        new LandAcquisitionStatus(SiteLandAcquisitionStatus.FullOwnership),
        SiteUsingModernMethodsOfConstruction.Yes);
}
