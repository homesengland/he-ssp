using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Organisation.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class SitePartnersBuilder : TestObjectBuilder<SitePartnersBuilder, SitePartners>
{
    private SitePartnersBuilder()
        : base(new SitePartners())
    {
    }

    protected override SitePartnersBuilder Builder => this;

    public static SitePartnersBuilder New() => new();

    public SitePartnersBuilder WithDevelopingPartner(InvestmentsOrganisation value) => SetProperty(x => x.DevelopingPartner, value);

    public SitePartnersBuilder WithOwnerOfTheLand(InvestmentsOrganisation value) => SetProperty(x => x.OwnerOfTheLand, value);

    public SitePartnersBuilder WithOwnerOfTheHomes(InvestmentsOrganisation value) => SetProperty(x => x.OwnerOfTheHomes, value);
}
