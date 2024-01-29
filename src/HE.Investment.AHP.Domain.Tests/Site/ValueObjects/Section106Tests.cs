using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects;

public class Section106Tests
{
    [Theory]
    [InlineData(false, null, null, null, null)]
    [InlineData(true, false, null, null, true)]
    [InlineData(true, false, null, null, false)]
    [InlineData(true, true, false, true, true)]
    [InlineData(true, true, false, true, false)]
    [InlineData(true, true, false, false, true)]
    [InlineData(true, true, false, false, false)]
    [InlineData(true, true, true, null, true)]
    [InlineData(true, true, true, null, false)]
    public void ShouldIsAnswered_ReturnTrue(
        bool? generalAgreement,
        bool? affordableHousing,
        bool? onlyAffordableHousing,
        bool? additionalAffordableHousing,
        bool? capitalFundingEligibility)
    {
        // given
        var section106 = new Section106(
            generalAgreement,
            affordableHousing,
            onlyAffordableHousing,
            additionalAffordableHousing,
            capitalFundingEligibility,
            null);

        // then
        section106.IsAnswered().Should().Be(true);
    }

    [Theory]
    [InlineData(true, null, null, null, null)]
    [InlineData(true, true, null, null, null)]
    [InlineData(true, true, true, null, null)]
    [InlineData(true, true, false, null, null)]
    [InlineData(true, true, true, true, null)]
    [InlineData(true, true, true, false, null)]
    [InlineData(true, true, false, true, null)]
    [InlineData(true, true, false, false, null)]
    public void ShouldIsAnswered_ReturnFalse(
        bool? generalAgreement,
        bool? affordableHousing,
        bool? onlyAffordableHousing,
        bool? additionalAffordableHousing,
        bool? capitalFundingEligibility)
    {
        // given
        var section106 = new Section106(
            generalAgreement,
            affordableHousing,
            onlyAffordableHousing,
            additionalAffordableHousing,
            capitalFundingEligibility,
            null);

        // then
        section106.IsAnswered().Should().Be(false);
    }
}