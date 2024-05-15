using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.SitePartnersTests;

public class WithOwnerOfTheHomesTests
{
    [Fact]
    public void ShouldThrowException_WhenConfirmationIsNotProvided()
    {
        // given
        var testCandidate = SitePartnersBuilder.New().Build();

        // when
        var provide = () => testCandidate.WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany, null);

        // then
        provide.Should().Throw<DomainValidationException>().WithMessage("Select yes if you want to confirm the owner of the homes after completion");
    }

    [Fact]
    public void ShouldReturnNotChangedPartners_WhenConfirmationIsNo()
    {
        // given
        var testCandidate = SitePartnersBuilder.New().WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany).Build();

        // when
        var result = testCandidate.WithOwnerOfTheHomes(InvestmentsOrganisationTestData.CactusDevelopments, false);

        // then
        result.Should().Be(testCandidate);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.JjCompany);
    }

    [Fact]
    public void ShouldReturnUpdatedPartners_WhenConfirmationIsYes()
    {
        // given
        var testCandidate = SitePartnersBuilder.New().WithOwnerOfTheHomes(InvestmentsOrganisationTestData.JjCompany).Build();

        // when
        var result = testCandidate.WithOwnerOfTheHomes(InvestmentsOrganisationTestData.CactusDevelopments, true);

        // then
        result.Should().NotBe(testCandidate);
        result.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        testCandidate.OwnerOfTheHomes.Should().Be(InvestmentsOrganisationTestData.JjCompany);
    }
}
