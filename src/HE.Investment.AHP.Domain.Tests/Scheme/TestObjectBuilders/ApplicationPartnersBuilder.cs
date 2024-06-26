using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investments.Organisation.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;

public class ApplicationPartnersBuilder : TestObjectBuilder<ApplicationPartnersBuilder, ApplicationPartners>
{
    private ApplicationPartnersBuilder()
        : base(new ApplicationPartners(
            InvestmentsOrganisationTestData.JjCompany,
            InvestmentsOrganisationTestData.JjCompany,
            InvestmentsOrganisationTestData.JjCompany))
    {
    }

    protected override ApplicationPartnersBuilder Builder => this;

    public static ApplicationPartnersBuilder New() => new();

    public ApplicationPartnersBuilder WithDevelopingPartner(InvestmentsOrganisation value) => SetProperty(x => x.DevelopingPartner, value);

    public ApplicationPartnersBuilder WithOwnerOfTheLand(InvestmentsOrganisation value) => SetProperty(x => x.OwnerOfTheLand, value);

    public ApplicationPartnersBuilder WithOwnerOfTheHomes(InvestmentsOrganisation value) => SetProperty(x => x.OwnerOfTheHomes, value);

    public ApplicationPartnersBuilder WithPartnersConfirmation(bool? value) => SetProperty(x => x.ArePartnersConfirmed, value);
}
