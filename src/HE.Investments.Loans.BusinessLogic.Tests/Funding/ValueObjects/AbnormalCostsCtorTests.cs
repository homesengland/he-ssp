using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.ValueObjects;
public class AbnormalCostsCtorTests
{
    [Fact]
    public void ShouldCreateAbnormalCostsWithAdditionalInformation()
    {
        // given
        var response = CommonResponse.Yes;
        var additionalInformation = "additional information";

        // when
        var action = () => AbnormalCosts.FromString(response, additionalInformation);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().IsAnyAbnormalCost.Should().BeTrue();
        action().AbnormalCostsAdditionalInformation.Should().Be(additionalInformation);
    }

    [Fact]
    public void ShouldCreateAbnormalCostsWithoutAdditionalInformation()
    {
        // given
        var response = CommonResponse.No;

        // when
        var action = () => AbnormalCosts.FromString(response, string.Empty);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().IsAnyAbnormalCost.Should().BeFalse();
        action().AbnormalCostsAdditionalInformation.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAbnormalCostIsTrueButNoAdditionalInformationIsProvided()
    {
        // given
        var response = true;
        string? additionalInformation = null;

        // when
        var action = () => AbnormalCosts.New(response, additionalInformation);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EnterMoreDetails);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenInputIsLongerThan1500Chars()
    {
        // given
        var response = true;
        var additionalInformation = new string('*', 1501);

        // when
        var action = () => AbnormalCosts.New(response, additionalInformation);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.AbnormalCostsInfo));
    }

    [Fact]
    public void ShouldAbnormalCostsAdditionalInformationBeEmpty_WhenIsAnyAbnormalCostIsFalse()
    {
        // given
        var response = false;
        var additionalInformation = "additional information";

        // when
        var abnormalCosts = AbnormalCosts.New(response, additionalInformation);

        // then
        abnormalCosts.AbnormalCostsAdditionalInformation.Should().BeNull();
        abnormalCosts.IsAnyAbnormalCost.Should().BeFalse();
    }
}
