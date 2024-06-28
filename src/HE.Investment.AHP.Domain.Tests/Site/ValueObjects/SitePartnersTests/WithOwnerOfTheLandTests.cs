using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.SitePartnersTests;

public class WithOwnerOfTheLandTests
{
    [Fact]
    public void ShouldThrowException_WhenConfirmationIsNotProvided()
    {
        // given
        var testCandidate = SitePartnersBuilder.New().Build();

        // when
        var provide = () => testCandidate.WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany, null);

        // then
        provide.Should().Throw<DomainValidationException>().WithMessage("Select yes to confirm the owner of the land during development");
    }

    [Fact]
    public void ShouldReturnNotChangedPartners_WhenConfirmationIsNo()
    {
        // given
        var testCandidate = SitePartnersBuilder.New().WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany).Build();

        // when
        var result = testCandidate.WithOwnerOfTheLand(InvestmentsOrganisationTestData.CactusDevelopments, false);

        // then
        result.Should().Be(testCandidate);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.JjCompany);
    }

    [Fact]
    public void ShouldReturnUpdatedPartners_WhenConfirmationIsYes()
    {
        // given
        var testCandidate = SitePartnersBuilder.New().WithOwnerOfTheLand(InvestmentsOrganisationTestData.JjCompany).Build();

        // when
        var result = testCandidate.WithOwnerOfTheLand(InvestmentsOrganisationTestData.CactusDevelopments, true);

        // then
        result.Should().NotBe(testCandidate);
        result.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        testCandidate.OwnerOfTheLand.Should().Be(InvestmentsOrganisationTestData.JjCompany);
    }
}
