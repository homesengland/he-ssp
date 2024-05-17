using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;
using HE.Investment.AHP.Domain.Tests.Site.TestData;

namespace HE.Investment.AHP.Domain.Tests.Scheme.ValueObjects.ApplicationPartnersTests;

public class WithDevelopingPartnerTests
{
    [Fact]
    public void ShouldNotChangeAnything_WhenDevelopingPartnerIsTheSame()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany)
            .WithConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany);

        // then
        result.Should().Be(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.IsConfirmed.Should().BeTrue();
    }

    [Fact]
    public void ShouldResetConfirmation_WhenDevelopingPartnerChanged()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany)
            .WithConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithDevelopingPartner(InvestmentsOrganisationTestData.CactusDevelopments);

        // then
        result.Should().NotBe(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.IsConfirmed.Should().BeNull();
    }
}
