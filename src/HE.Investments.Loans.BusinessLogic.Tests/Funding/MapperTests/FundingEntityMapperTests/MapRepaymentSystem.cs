using HE.Investments.Loans.BusinessLogic.Funding.Mappers;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.MapperTests.FundingEntityMapperTests;
public class MapRepaymentSystem
{
    [Fact]
    public void ShouldReturnFilledRepaymentSystem_WhenDataAreProvided()
    {
        // given
        var refinanceValue = FundingFormOption.Refinance;
        var refinanceAdditionalInformation = "refinance additional information";

        // when
        var repaymentSystem = FundingEntityMapper.MapRepaymentSystem(refinanceValue, refinanceAdditionalInformation);

        // then
        repaymentSystem!.Refinance!.Value.Should().Be(refinanceValue);
        repaymentSystem!.Refinance!.AdditionalInformation.Should().Be(refinanceAdditionalInformation);
    }

    [Fact]
    public void ShouldReturnRepaymentSystemValue_WhenRepaymentSystemValueObjectIsGiven()
    {
        // given
        var repaymentSystem = RepaymentSystem.New(FundingFormOption.Repay, null);

        // when
        var repayValue = FundingEntityMapper.MapRepaymentSystem(repaymentSystem);

        // then
        repayValue!.Should().Be(repaymentSystem.Repay!.Value);
    }

    [Fact]
    public void ShouldReturnNull_WhenProvidedDataAreMissing()
    {
        // given
        string? refinanceValue = null;
        var refinanceAdditionalInformation = "refinance additional information";

        // when
        var repaymentSystem = FundingEntityMapper.MapRepaymentSystem(refinanceValue, refinanceAdditionalInformation);

        // then
        repaymentSystem.Should().BeNull();
    }
}
