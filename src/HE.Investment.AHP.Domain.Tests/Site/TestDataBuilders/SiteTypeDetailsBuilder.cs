using HE.Investment.AHP.Contract.Site;
using HE.Investments.TestsUtils.TestFramework;
using SiteTypeDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteTypeDetails;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class SiteTypeDetailsBuilder : TestObjectBuilder<SiteTypeDetailsBuilder, SiteTypeDetails>
{
    private SiteTypeDetailsBuilder()
        : base(new SiteTypeDetails())
    {
    }

    protected override SiteTypeDetailsBuilder Builder => this;

    public static SiteTypeDetailsBuilder New() => new();

    public SiteTypeDetailsBuilder WithSiteType(SiteType value) => SetProperty(x => x.SiteType, value);

    public SiteTypeDetailsBuilder WithIsOnGreenBelt(bool value) => SetProperty(x => x.IsOnGreenBelt, value);

    public SiteTypeDetailsBuilder WithIsRegenerationSite(bool value) => SetProperty(x => x.IsRegenerationSite, value);
}
