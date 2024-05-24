using FluentAssertions;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investment.AHP.Domain.Tests.Site.TestData;

namespace HE.Investment.AHP.Domain.Tests.Scheme.ValueObjects.ApplicationPartnersTests;

public class ConfirmedPartnerTests
{
    [Fact]
    public void ShouldCreateConfirmedApplicationPartners_WhenFullOrganisationIsProvided()
    {
        // given
        var organisation = new OrganisationBasicInfoBuilder()
            .WithId(InvestmentsOrganisationTestData.CactusDevelopments.Id)
            .WithName(InvestmentsOrganisationTestData.CactusDevelopments.Name)
            .Build();

        // when
        var result = ApplicationPartners.ConfirmedPartner(organisation);

        // then
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        result.ArePartnersConfirmed.Should().BeTrue();
    }

    [Fact]
    public void ShouldCreateConfirmedApplicationPartners_WhenOnlyOrganisationIdIsProvided()
    {
        // given & when
        var result = ApplicationPartners.ConfirmedPartner(InvestmentsOrganisationTestData.CactusDevelopments.Id);

        // then
        result.DevelopingPartner.Id.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Id);
        result.OwnerOfTheLand.Id.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Id);
        result.OwnerOfTheHomes.Id.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Id);
        result.ArePartnersConfirmed.Should().BeTrue();
    }
}
