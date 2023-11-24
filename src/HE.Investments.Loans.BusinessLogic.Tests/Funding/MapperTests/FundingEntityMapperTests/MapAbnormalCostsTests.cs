using HE.Investments.Loans.BusinessLogic.Funding.Mappers;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.MapperTests.FundingEntityMapperTests;
public class MapAbnormalCostsTests
{
    [Fact]
    public void ShouldReturnFilledAbnormalCosts_WhenDataAreFilled()
    {
        // given
        var isAnyAbnormalCost = true;
        var abnormalCostsAdditionalInformation = "additional information";

        // when
        var abnormalCosts = FundingEntityMapper.MapAbnormalCosts(isAnyAbnormalCost, abnormalCostsAdditionalInformation);

        // then
        abnormalCosts!.IsAnyAbnormalCost.Should().Be(isAnyAbnormalCost);
        abnormalCosts!.AbnormalCostsAdditionalInformation.Should().Be(abnormalCostsAdditionalInformation);
    }

    [Fact]
    public void ShouldReturnNull_WhenProvidedDataAreMissing()
    {
        // given
        bool? isAnyAbnormalCost = null;
        var abnormalCostsAdditionalInformation = "additional information";

        // when
        var abnormalCosts = FundingEntityMapper.MapAbnormalCosts(isAnyAbnormalCost, abnormalCostsAdditionalInformation);

        // then
        abnormalCosts.Should().BeNull();
    }
}
