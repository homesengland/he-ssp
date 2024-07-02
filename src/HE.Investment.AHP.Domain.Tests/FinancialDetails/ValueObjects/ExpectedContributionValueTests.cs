using FluentAssertions;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.ValueObjects;

public class ExpectedContributionValueTests
{
    [Theory]
    [InlineData(ExpectedContributionFields.RentalIncomeBorrowing)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnThisScheme)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnOtherSchemes)]
    [InlineData(ExpectedContributionFields.OwnResources)]
    [InlineData(ExpectedContributionFields.RcgfContribution)]
    [InlineData(ExpectedContributionFields.OtherCapitalSources)]
    [InlineData(ExpectedContributionFields.SharedOwnershipSales)]
    [InlineData(ExpectedContributionFields.HomesTransferValue)]
    public void ShouldThrowDomainValidationException_WhenValueIsEmpty(ExpectedContributionFields field)
    {
        // given && when
        var action = () => new ExpectedContributionValue(field, string.Empty);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .WithMessage($"Enter the {field.GetDescription()}, in pounds");
    }

    [Theory]
    [InlineData(ExpectedContributionFields.RentalIncomeBorrowing)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnThisScheme)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnOtherSchemes)]
    [InlineData(ExpectedContributionFields.OwnResources)]
    [InlineData(ExpectedContributionFields.RcgfContribution)]
    [InlineData(ExpectedContributionFields.OtherCapitalSources)]
    [InlineData(ExpectedContributionFields.SharedOwnershipSales)]
    [InlineData(ExpectedContributionFields.HomesTransferValue)]
    public void ShouldThrowDomainValidationException_WhenValueIsNegative(ExpectedContributionFields field)
    {
        // given && when
        var action = () => new ExpectedContributionValue(field, "-1");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == $"The {field.GetDescription()} must be 0 or more");
    }

    [Theory]
    [InlineData(ExpectedContributionFields.RentalIncomeBorrowing)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnThisScheme)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnOtherSchemes)]
    [InlineData(ExpectedContributionFields.OwnResources)]
    [InlineData(ExpectedContributionFields.RcgfContribution)]
    [InlineData(ExpectedContributionFields.OtherCapitalSources)]
    [InlineData(ExpectedContributionFields.SharedOwnershipSales)]
    [InlineData(ExpectedContributionFields.HomesTransferValue)]
    public void ShouldThrowDomainValidationException_WhenValueIsOutOfRange(ExpectedContributionFields field)
    {
        // given && when
        var action = () => new ExpectedContributionValue(field, "1000000000");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == $"The {field.GetDescription()} must be 999999999 or fewer");
    }

    [Theory]
    [InlineData(ExpectedContributionFields.RentalIncomeBorrowing)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnThisScheme)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnOtherSchemes)]
    [InlineData(ExpectedContributionFields.OwnResources)]
    [InlineData(ExpectedContributionFields.RcgfContribution)]
    [InlineData(ExpectedContributionFields.OtherCapitalSources)]
    [InlineData(ExpectedContributionFields.SharedOwnershipSales)]
    [InlineData(ExpectedContributionFields.HomesTransferValue)]
    public void ShouldThrowDomainValidationException_WhenValueIsDecimal(ExpectedContributionFields field)
    {
        // given && when
        var action = () => new ExpectedContributionValue(field, "10.234");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == $"The {field.GetDescription()} must not include pence, like 300");
    }

    [Theory]
    [InlineData(ExpectedContributionFields.RentalIncomeBorrowing)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnThisScheme)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnOtherSchemes)]
    [InlineData(ExpectedContributionFields.OwnResources)]
    [InlineData(ExpectedContributionFields.RcgfContribution)]
    [InlineData(ExpectedContributionFields.OtherCapitalSources)]
    [InlineData(ExpectedContributionFields.SharedOwnershipSales)]
    [InlineData(ExpectedContributionFields.HomesTransferValue)]
    public void ShouldThrowDomainValidationException_WhenValueIsNotANumber(ExpectedContributionFields field)
    {
        // given && when
        var action = () => new ExpectedContributionValue(field, "abc");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == $"The {field.GetDescription()} must be a whole number, like 300");
    }

    [Theory]
    [InlineData(ExpectedContributionFields.RentalIncomeBorrowing, "0", 0)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnThisScheme, "100", 100)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnOtherSchemes, "999999999", 999999999)]
    public void ShouldCreateExpectedContributionValue_WhenValueIsValid(ExpectedContributionFields field, string input, int expectedValue)
    {
        // given && when
        var grantValue = new ExpectedContributionValue(field, input);

        // then
        grantValue.Value.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(ExpectedContributionFields.RentalIncomeBorrowing, 100)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnThisScheme, 200)]
    [InlineData(ExpectedContributionFields.SaleOfHomesOnOtherSchemes, 300)]
    public void ShouldCreateExpectedContributionValue_WhenIntValueIsValid(ExpectedContributionFields field, int input)
    {
        // given && when
        var grantValue = new ExpectedContributionValue(field, input);

        // then
        grantValue.Value.Should().Be(input);
    }
}
