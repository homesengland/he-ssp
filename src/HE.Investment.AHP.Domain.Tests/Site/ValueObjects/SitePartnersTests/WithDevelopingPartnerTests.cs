using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.SitePartnersTests;

public class WithDevelopingPartnerTests
{
    [Fact]
    public void ShouldThrowException_WhenConfirmationIsNotProvided()
    {
        // given
        var testCandidate = SitePartnersBuilder.New().Build();

        // when
        var provide = () => testCandidate.WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany, null);

        // then
        provide.Should().Throw<DomainValidationException>().WithMessage("Select yes to confirm the developing partner");
    }

    [Fact]
    public void ShouldReturnNotChangedPartners_WhenConfirmationIsNo()
    {
        // given
        var testCandidate = SitePartnersBuilder.New().WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany).Build();

        // when
        var result = testCandidate.WithDevelopingPartner(InvestmentsOrganisationTestData.CactusDevelopments, false);

        // then
        result.Should().Be(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
    }

    [Fact]
    public void ShouldReturnUpdatedPartners_WhenConfirmationIsYes()
    {
        // given
        var testCandidate = SitePartnersBuilder.New().WithDevelopingPartner(InvestmentsOrganisationTestData.JjCompany).Build();

        // when
        var result = testCandidate.WithDevelopingPartner(InvestmentsOrganisationTestData.CactusDevelopments, true);

        // then
        result.Should().NotBe(testCandidate);
        result.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments);
        testCandidate.DevelopingPartner.Should().Be(InvestmentsOrganisationTestData.JjCompany);
    }
}
