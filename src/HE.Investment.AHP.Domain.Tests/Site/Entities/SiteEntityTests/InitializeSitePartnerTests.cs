using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Site.Entities.SiteEntityTests;

public class InitializeSitePartnerTests
{
    [Fact]
    public void ShouldInitializeSitePartners_WhenSitePartnerIsNotInitializedAndOrganisationIsNotConsortiumMember()
    {
        // given
        var testCandidate = SiteEntityBuilder.New().Build();
        var organisation = new OrganisationBasicInfoBuilder().Build();

        // when
        testCandidate.InitializeSitePartner(false, organisation);

        // then
        testCandidate.SitePartners.DevelopingPartner!.Id.Should().Be(organisation.OrganisationId);
        testCandidate.SitePartners.DevelopingPartner.Name.Should().Be(organisation.RegisteredCompanyName);
        testCandidate.SitePartners.OwnerOfTheLand!.Id.Should().Be(organisation.OrganisationId);
        testCandidate.SitePartners.OwnerOfTheLand.Name.Should().Be(organisation.RegisteredCompanyName);
        testCandidate.SitePartners.OwnerOfTheHomes!.Id.Should().Be(organisation.OrganisationId);
        testCandidate.SitePartners.OwnerOfTheHomes.Name.Should().Be(organisation.RegisteredCompanyName);
    }

    [Fact]
    public void ShouldNotInitializeSitePartners_WhenSitePartnerAreAlreadyInitialized()
    {
        // given
        var sitePartners = SitePartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.CactusDevelopments)
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.MoralesEntertainment)
            .Build();

        var testCandidate = SiteEntityBuilder.New().WithSitePartners(sitePartners).Build();
        var organisation = new OrganisationBasicInfoBuilder().Build();

        // when
        testCandidate.InitializeSitePartner(false, organisation);

        // then
        testCandidate.SitePartners.DevelopingPartner!.Id.Should().Be(InvestmentsOrganisationTestData.JjCompany.Id);
        testCandidate.SitePartners.DevelopingPartner.Name.Should().Be(InvestmentsOrganisationTestData.JjCompany.Name);
        testCandidate.SitePartners.OwnerOfTheLand!.Id.Should().Be(InvestmentsOrganisationTestData.MoralesEntertainment.Id);
        testCandidate.SitePartners.OwnerOfTheLand.Name.Should().Be(InvestmentsOrganisationTestData.MoralesEntertainment.Name);
        testCandidate.SitePartners.OwnerOfTheHomes!.Id.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Id);
        testCandidate.SitePartners.OwnerOfTheHomes.Name.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Name);
    }

    [Fact]
    public void ShouldNotInitializeSitePartners_WhenOrganisationIsConsortiumMember()
    {
        // given
        var testCandidate = SiteEntityBuilder.New().Build();
        var organisation = new OrganisationBasicInfoBuilder().Build();

        // when
        testCandidate.InitializeSitePartner(true, organisation);

        // then
        testCandidate.SitePartners.DevelopingPartner.Should().BeNull();
        testCandidate.SitePartners.OwnerOfTheLand.Should().BeNull();
        testCandidate.SitePartners.OwnerOfTheHomes.Should().BeNull();
    }
}
