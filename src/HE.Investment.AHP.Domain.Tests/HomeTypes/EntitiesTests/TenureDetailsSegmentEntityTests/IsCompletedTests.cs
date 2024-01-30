using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.EntitiesTests.TenureDetailsSegmentEntityTests;

public class IsCompletedTests
{
    [Theory]
    [InlineData(1000000, 1000, YesNoType.Yes, "justification")]
    [InlineData(1000000, 1000, YesNoType.No, null)]
    public void ShouldReturnTrue_WhenTenureIsSocialRentAndAllRequiredFieldsAreProvided(
        int marketValue,
        int prospectiveRent,
        YesNoType exemptFromTheRightToSharedOwnership,
        string? exemptionJustification)
    {
        // given
        var tenureDetailsBuilder = new TenureDetailsTestDataBuilder()
            .WithMarketValue(new MarketValue(marketValue))
            .WithProspectiveRent(new ProspectiveRent(prospectiveRent))
            .WithExemptFromTheRightToSharedOwnership(exemptFromTheRightToSharedOwnership);

        if (exemptionJustification != null)
        {
            tenureDetailsBuilder = tenureDetailsBuilder.WithExemptionJustification(exemptionJustification);
        }

        // when
        var result = tenureDetailsBuilder.Build().IsCompleted(HousingType.General, Tenure.SocialRent);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(1000000, 1000, YesNoType.Yes, null)]
    [InlineData(1000000, null, YesNoType.No, null)]
    [InlineData(null, 1000, YesNoType.No, null)]
    public void ShouldReturnFalse_WhenTenureIsSocialRentAndAllRequiredFieldsAreProvided(
        int? marketValue,
        int? prospectiveRent,
        YesNoType exemptFromTheRightToSharedOwnership,
        string? exemptionJustification)
    {
        // given
        var tenureDetailsBuilder = new TenureDetailsTestDataBuilder().WithExemptFromTheRightToSharedOwnership(exemptFromTheRightToSharedOwnership);

        if (marketValue != null)
        {
            tenureDetailsBuilder = tenureDetailsBuilder.WithMarketValue(new MarketValue(marketValue.Value));
        }

        if (prospectiveRent != null)
        {
            tenureDetailsBuilder = tenureDetailsBuilder.WithProspectiveRent(new ProspectiveRent(prospectiveRent.Value));
        }

        if (exemptionJustification != null)
        {
            tenureDetailsBuilder = tenureDetailsBuilder.WithExemptionJustification(exemptionJustification);
        }

        // when
        var result = tenureDetailsBuilder.Build().IsCompleted(HousingType.General, Tenure.SocialRent);

        // then
        result.Should().BeFalse();
    }
}
