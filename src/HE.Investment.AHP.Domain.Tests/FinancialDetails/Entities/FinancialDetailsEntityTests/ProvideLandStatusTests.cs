using FluentAssertions;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.Tests.FinancialDetails.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.Entities.FinancialDetailsEntityTests;

public class ProvideLandStatusTests
{
    [Fact]
    public void ShouldSetLandStatus_WhenExpectedPurchasePriceIsProvidedAndSiteLandAcquisitionStatusIsNotFullOwnership()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .WithSiteBasicInfo(SiteLandAcquisitionStatus.PurchaseNegotiationsNotStarted)
            .Build();

        var landStatus = new LandStatus(null, new ExpectedPurchasePrice("40"));

        // when
        financialDetailsEntity.ProvideLandStatus(landStatus);

        // then
        financialDetailsEntity.LandStatus.ExpectedPurchasePrice.Should().Be(landStatus.ExpectedPurchasePrice);
        financialDetailsEntity.LandStatus.PurchasePrice.Should().BeNull();
    }

    [Fact]
    public void ShouldSetLandStatus_WhenExpectedPurchasePriceIsProvidedAndSiteLandAcquisitionStatusIsFullOwnership()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .WithSiteBasicInfo(SiteLandAcquisitionStatus.FullOwnership)
            .Build();

        var landStatus = new LandStatus(new PurchasePrice("30"), null);

        // when
        financialDetailsEntity.ProvideLandStatus(landStatus);

        // then
        financialDetailsEntity.LandStatus.ExpectedPurchasePrice.Should().BeNull();
        financialDetailsEntity.LandStatus.PurchasePrice.Should().Be(landStatus.PurchasePrice);
    }

    [Fact]
    public void ShouldThrowException_WhenExpectedPurchasePriceIsProvidedAndSiteLandAcquisitionStatusIsFullOwnership()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .WithSiteBasicInfo(SiteLandAcquisitionStatus.FullOwnership)
            .Build();

        var landStatus = new LandStatus(null, new ExpectedPurchasePrice("40"));

        // when
        var action = () => financialDetailsEntity.ProvideLandStatus(landStatus);

        // then
        action.Should().Throw<DomainValidationException>();
    }
}
