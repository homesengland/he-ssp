using FluentAssertions;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Tests.Common.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Site.Entities.SiteEntityTests;

public class NewSiteTests
{
    [Fact]
    public void ShouldSetMyOrganisationAsAllPartners_WhenOrganisationIsNotPartOfConsortium()
    {
        // given
        var userAccount = AhpUserAccountTestData.UserAccountOneNoConsortium;
        var myOrganisation = new InvestmentsOrganisation(userAccount.SelectedOrganisationId(), userAccount.SelectedOrganisation().RegisteredCompanyName);

        // when
        var result = SiteEntity.NewSite(userAccount, new FrontDoorProjectId("1"), null);

        // then
        result.SitePartners.DevelopingPartner.Should().Be(myOrganisation);
        result.SitePartners.OwnerOfTheLand.Should().Be(myOrganisation);
        result.SitePartners.OwnerOfTheHomes.Should().Be(myOrganisation);
    }

    [Fact]
    public void ShouldNotSetPartners_WhenOrganisationIsPartOfConsortium()
    {
        // given
        var userAccount = AhpUserAccountTestData.UserAccountOneWithConsortium;

        // when
        var result = SiteEntity.NewSite(userAccount, new FrontDoorProjectId("1"), null);

        // then
        result.SitePartners.DevelopingPartner.Should().BeNull();
        result.SitePartners.OwnerOfTheLand.Should().BeNull();
        result.SitePartners.OwnerOfTheHomes.Should().BeNull();
    }
}
