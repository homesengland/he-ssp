using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Scheme.ValueObjects.ApplicationPartnersTests;

// TODO: add tests for null, false confirmation
public class WithOwnerOfTheLandTests
{
    [Fact]
    public void ShouldNotChangeAnything_WhenOwnerOfTheLandIsTheSame()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany)
            .WithAllPartnersConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany, isPartnerConfirmed: true);

        // then
        result.Should().Be(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.AreAllPartnersConfirmed.Should().BeTrue();
    }

    [Fact]
    public void ShouldResetConfirmation_WhenOwnerOfTheLandChanged()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany)
            .WithAllPartnersConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithOwnerOfTheLand(InvestmentsOrganisationTestData.CactusDevelopments, isPartnerConfirmed: true);

        // then
        result.Should().NotBe(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.AreAllPartnersConfirmed.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowException_WhenConfirmationIsNotProvided()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New().Build();

        // when
        var provide = () => testCandidate.WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany, null);

        // then
        provide.Should().Throw<DomainValidationException>().WithMessage("Select yes if you want to confirm the owner of the land during development");
    }

    [Fact]
    public void ShouldReturnNotChangedPartners_WhenConfirmationIsNo()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany)
            .WithAllPartnersConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithOwnerOfTheLand(InvestmentsOrganisationTestData.CactusDevelopments, false);

        // then
        result.Should().Be(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.AreAllPartnersConfirmed.Should().BeTrue();
    }
}
