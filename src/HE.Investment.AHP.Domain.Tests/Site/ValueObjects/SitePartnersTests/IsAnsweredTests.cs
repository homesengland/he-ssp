extern alias Org;

using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Contract;
using Org::HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.SitePartnersTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenAllPartnersAreProvided()
    {
        // given
        var testCandidate = SitePartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.CactusDevelopments)
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("a", "b", null)]
    [InlineData("a", null, "c")]
    [InlineData(null, "b", "c")]
    [InlineData(null, null, null)]
    public void ShouldReturnFalse_WhenSomePartnersAreNotProvided(string? developingPartner, string? ownerOfTheLand, string? ownerOfTheHomes)
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

        var testCandidate = builder.Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
