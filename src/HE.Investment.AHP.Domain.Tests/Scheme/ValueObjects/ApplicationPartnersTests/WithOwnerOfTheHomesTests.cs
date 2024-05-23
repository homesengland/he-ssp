using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Scheme.ValueObjects.ApplicationPartnersTests;

public class WithOwnerOfTheHomesTests
{
    [Fact]
    public void ShouldNotChangeAnything_WhenOwnerOfTheHomesIsTheSame()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany)
            .WithPartnersConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany, isPartnerConfirmed: true);

        // then
        result.Should().Be(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.ArePartnersConfirmed.Should().BeTrue();
    }

    [Fact]
    public void ShouldResetConfirmation_WhenOwnerOfTheHomesChanged()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany)
            .WithPartnersConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithOwnerOfTheHomes(InvestmentsOrganisationTestData.CactusDevelopments, isPartnerConfirmed: true);

        // then
        result.Should().NotBe(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        result.ArePartnersConfirmed.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowException_WhenConfirmationIsNotProvided()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New().Build();

        // when
        var provide = () => testCandidate.WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany, null);

        // then
        provide.Should().Throw<DomainValidationException>().WithMessage("Select yes if you want to confirm the owner of the homes after completion");
    }

    [Fact]
    public void ShouldReturnNotChangedPartners_WhenConfirmationIsNo()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany)
            .WithPartnersConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithOwnerOfTheHomes(InvestmentsOrganisationTestData.CactusDevelopments, false);

        // then
        result.Should().Be(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.ArePartnersConfirmed.Should().BeTrue();
    }
}
