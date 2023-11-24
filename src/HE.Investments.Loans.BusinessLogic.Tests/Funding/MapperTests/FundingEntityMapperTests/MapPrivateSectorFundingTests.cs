using HE.Investments.Loans.BusinessLogic.Funding.Mappers;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.MapperTests.FundingEntityMapperTests;
public class MapPrivateSectorFundingTests
{
    [Fact]
    public void ShouldReturnFilledPrivateSectorFunding_WhenDataAreProvided()
    {
        // given
        var isApplied = true;
        var privateSectorFundingApplyResult = "positive result";

        // when
        var privateSectorFunding = FundingEntityMapper.MapPrivateSectorFunding(isApplied, privateSectorFundingApplyResult);

        // then
        privateSectorFunding!.IsApplied.Should().Be(isApplied);
        privateSectorFunding!.PrivateSectorFundingApplyResult.Should().Be(privateSectorFundingApplyResult);
    }

    [Fact]
    public void ShouldReturnNotApplyingReason_WhenPrivateSectorFundingIsProvidedButNotApplied()
    {
        // given
        var privateSectorFunding = PrivateSectorFunding.New(false, null, "not applying reason");

        // when
        var notApplyingReason = FundingEntityMapper.MapPrivateSectorFundingAdditionalInformation(privateSectorFunding);

        // then
        notApplyingReason.Should().Be(privateSectorFunding.PrivateSectorFundingNotApplyingReason);
    }

    [Fact]
    public void ShouldReturnNull_WhenProvidedDataAreMissing()
    {
        // given
        bool? isApplied = null;
        var privateSectorFundingApplyResult = "result";

        // when
        var privateSectorFunding = FundingEntityMapper.MapPrivateSectorFunding(isApplied, privateSectorFundingApplyResult);

        // then
        privateSectorFunding.Should().BeNull();
    }
}
