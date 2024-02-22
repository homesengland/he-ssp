using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class SiteUseDetailsBuilder : TestObjectBuilder<SiteUseDetailsBuilder, SiteUseDetails>
{
    private SiteUseDetailsBuilder()
        : base(new SiteUseDetails())
    {
    }

    protected override SiteUseDetailsBuilder Builder => this;

    public static SiteUseDetailsBuilder New() => new();

    public SiteUseDetailsBuilder WithIsPartOfStreetFrontInfill(bool value) => SetProperty(x => x.IsPartOfStreetFrontInfill, value);

    public SiteUseDetailsBuilder WithIsForTravellerPitchSite(bool value) => SetProperty(x => x.IsForTravellerPitchSite, value);
}
