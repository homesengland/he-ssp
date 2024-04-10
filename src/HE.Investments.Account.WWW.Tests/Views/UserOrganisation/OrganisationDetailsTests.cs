using HE.Investments.Account.Contract.Organisation;

namespace HE.Investments.Account.WWW.Tests.Views.UserOrganisation;

public class OrganisationDetailsTests : AccountViewTestBase
{
    private readonly string _viewPath = "/Views/UserOrganisation/OrganisationDetails.cshtml";

    [Theory]
    [InlineData(InvestmentPartnerStatus.InvestmentPartnerFull, "Awesome Company has full Investment Partner status")]
    [InlineData(InvestmentPartnerStatus.InvestmentPartnerRestricted, "Awesome Company is a restricted Investment Partner")]
    [InlineData(InvestmentPartnerStatus.NotAnInvestmentPartner, "Awesome Company is not an Investment Partner")]
    public async Task ShouldDisplayInvestmentPartnerStatus_WhenOrganisationHasAnyAhpApplication(InvestmentPartnerStatus partnerStatus, string expectedPartnerStatus)
    {
        // given
        var model = new OrganisationDetails
        {
            Name = "Awesome Company",
            InvestmentPartnerStatus = partnerStatus,
            HasAnyAhpApplication = true,
        };

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasTitle("Manage Awesome Company details")
            .HasHeader2("Organisation details")
            .HasHeader2("Organisation status")
            .HasParagraph(expectedPartnerStatus);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData(InvestmentPartnerStatus.Undefined, true)]
    [InlineData(InvestmentPartnerStatus.NotAnInvestmentPartner, false)]
    public async Task ShouldNotDisplayInvestmentPartnerStatus_WhenInvestmentPartnerStatusIs(InvestmentPartnerStatus? partnerStatus, bool hasAnyAhpApplication)
    {
        // given
        var model = new OrganisationDetails
        {
            Name = "Awesome Company",
            InvestmentPartnerStatus = partnerStatus,
            HasAnyAhpApplication = hasAnyAhpApplication,
        };

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasTitle("Manage Awesome Company details")
            .HasHeader2("Organisation details")
            .HasHeader2("Organisation status", exists: false);
    }
}
