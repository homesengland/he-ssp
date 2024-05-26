using FluentAssertions;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Scheme.ValueObjects.ApplicationPartnersTests;

public class FromSitePartnersTests
{
    [Theory]
    [InlineData("a", "b", null)]
    [InlineData("a", null, "c")]
    [InlineData(null, "b", "c")]
    [InlineData(null, null, null)]
    public void ShouldThrowException_WhenSitePartnersIsNotCompleted(string? developingPartner, string? ownerOfTheLand, string? ownerOfTheHomes)
    {
        // given
        var builder = SitePartnersBuilder.New();
        if (developingPartner != null)
        {
            builder = builder.WithDevelopingPartner(new InvestmentsOrganisation(new OrganisationId(developingPartner), developingPartner));
        }

        if (ownerOfTheLand != null)
        {
            builder = builder.WithOwnerOfTheLand(new InvestmentsOrganisation(new OrganisationId(ownerOfTheLand), ownerOfTheLand));
        }

        if (ownerOfTheHomes != null)
        {
            builder = builder.WithOwnerOfTheHomes(new InvestmentsOrganisation(new OrganisationId(ownerOfTheHomes), ownerOfTheHomes));
        }

        var sitePartners = builder.Build();

        // when
        var create = () => ApplicationPartners.FromSitePartners(sitePartners);

        // then
        create.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldCreateNotConfirmedApplicationPartners_WhenSitePartnersIsCompleted()
    {
        // given
        var sitePartners = SitePartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.CactusDevelopments)
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.MoralesEntertainment)
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany)
            .Build();

        // when
        var result = ApplicationPartners.FromSitePartners(sitePartners);

        // then
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.MoralesEntertainment);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.ArePartnersConfirmed.Should().BeNull();
    }
}
