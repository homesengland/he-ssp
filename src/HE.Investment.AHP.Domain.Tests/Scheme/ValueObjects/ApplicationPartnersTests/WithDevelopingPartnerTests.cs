using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investments.Common.Contract.Exceptions;

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
            .WithAllPartnersConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany, isPartnerConfirmed: true);

        // then
        result.Should().Be(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.AreAllPartnersConfirmed.Should().BeTrue();
    }

    [Fact]
    public void ShouldResetConfirmation_WhenDevelopingPartnerChanged()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany)
            .WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany)
            .WithAllPartnersConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithDevelopingPartner(InvestmentsOrganisationTestData.CactusDevelopments, isPartnerConfirmed: true);

        // then
        result.Should().NotBe(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.AreAllPartnersConfirmed.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowException_WhenConfirmationIsNotProvided()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New().Build();

        // when
        var provide = () => testCandidate.WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany, null);

        // then
        provide.Should().Throw<DomainValidationException>().WithMessage("Select yes if you want to confirm the developing partner");
    }

    [Fact]
    public void ShouldReturnNotChangedPartners_WhenConfirmationIsNo()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithAllPartnersConfirmation(true)
            .Build();

        // when
        var result = testCandidate.WithDevelopingPartner(InvestmentsOrganisationTestData.CactusDevelopments, false);

        // then
        result.Should().Be(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
        result.AreAllPartnersConfirmed.Should().BeTrue();
    }
}
