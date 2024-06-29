using FluentAssertions;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.ValueObjects;

public class PublicGrantValueTests
{
    [Theory]
    [InlineData(PublicGrantFields.CountyCouncilGrants)]
    [InlineData(PublicGrantFields.DhscExtraCareGrants)]
    [InlineData(PublicGrantFields.LocalAuthorityGrants)]
    [InlineData(PublicGrantFields.SocialServicesGrants)]
    [InlineData(PublicGrantFields.HealthRelatedGrants)]
    [InlineData(PublicGrantFields.LotteryGrants)]
    [InlineData(PublicGrantFields.OtherPublicBodiesGrants)]
    public void ShouldThrowDomainValidationException_WhenValueIsEmpty(PublicGrantFields field)
    {
        // given && when
        var action = () => new PublicGrantValue(field, string.Empty);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .WithMessage($"Enter the {field.GetDescription()}, in pounds");
    }

    [Theory]
    [InlineData(PublicGrantFields.CountyCouncilGrants)]
    [InlineData(PublicGrantFields.DhscExtraCareGrants)]
    [InlineData(PublicGrantFields.LocalAuthorityGrants)]
    [InlineData(PublicGrantFields.SocialServicesGrants)]
    [InlineData(PublicGrantFields.HealthRelatedGrants)]
    [InlineData(PublicGrantFields.LotteryGrants)]
    [InlineData(PublicGrantFields.OtherPublicBodiesGrants)]
    public void ShouldThrowDomainValidationException_WhenValueIsNegative(PublicGrantFields field)
    {
        // given && when
        var action = () => new PublicGrantValue(field, "-1");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == $"The {field.GetDescription()} must be 0 or more");
    }

    [Theory]
    [InlineData(PublicGrantFields.CountyCouncilGrants)]
    [InlineData(PublicGrantFields.DhscExtraCareGrants)]
    [InlineData(PublicGrantFields.LocalAuthorityGrants)]
    [InlineData(PublicGrantFields.SocialServicesGrants)]
    [InlineData(PublicGrantFields.HealthRelatedGrants)]
    [InlineData(PublicGrantFields.LotteryGrants)]
    [InlineData(PublicGrantFields.OtherPublicBodiesGrants)]
    public void ShouldThrowDomainValidationException_WhenValueIsOutOfRange(PublicGrantFields field)
    {
        // given && when
        var action = () => new PublicGrantValue(field, "1000000000");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == $"The {field.GetDescription()} must be 999999999 or fewer");
    }

    [Theory]
    [InlineData(PublicGrantFields.CountyCouncilGrants)]
    [InlineData(PublicGrantFields.DhscExtraCareGrants)]
    [InlineData(PublicGrantFields.LocalAuthorityGrants)]
    [InlineData(PublicGrantFields.SocialServicesGrants)]
    [InlineData(PublicGrantFields.HealthRelatedGrants)]
    [InlineData(PublicGrantFields.LotteryGrants)]
    [InlineData(PublicGrantFields.OtherPublicBodiesGrants)]
    public void ShouldThrowDomainValidationException_WhenValueIsDecimal(PublicGrantFields field)
    {
        // given && when
        var action = () => new PublicGrantValue(field, "10.234");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == $"The {field.GetDescription()} must not include pence, like 300");
    }

    [Theory]
    [InlineData(PublicGrantFields.CountyCouncilGrants)]
    [InlineData(PublicGrantFields.DhscExtraCareGrants)]
    [InlineData(PublicGrantFields.LocalAuthorityGrants)]
    [InlineData(PublicGrantFields.SocialServicesGrants)]
    [InlineData(PublicGrantFields.HealthRelatedGrants)]
    [InlineData(PublicGrantFields.LotteryGrants)]
    [InlineData(PublicGrantFields.OtherPublicBodiesGrants)]
    public void ShouldThrowDomainValidationException_WhenValueIsNotANumber(PublicGrantFields field)
    {
        // given && when
        var action = () => new PublicGrantValue(field, "abc");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == $"The {field.GetDescription()} must be a whole number, like 300");
    }

    [Theory]
    [InlineData(PublicGrantFields.CountyCouncilGrants, "0", 0)]
    [InlineData(PublicGrantFields.DhscExtraCareGrants, "100", 100)]
    [InlineData(PublicGrantFields.LocalAuthorityGrants, "999999999", 999999999)]
    public void ShouldCreatePublicGrantValue_WhenValueIsValid(PublicGrantFields field, string input, int expectedValue)
    {
        // given && when
        var grantValue = new PublicGrantValue(field, input);

        // then
        grantValue.Value.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(PublicGrantFields.CountyCouncilGrants, 100)]
    [InlineData(PublicGrantFields.DhscExtraCareGrants, 200)]
    [InlineData(PublicGrantFields.LocalAuthorityGrants, 300)]
    public void ShouldCreatePublicGrantValue_WhenIntValueIsValid(PublicGrantFields field, int input)
    {
        // given && when
        var grantValue = new PublicGrantValue(field, input);

        // then
        grantValue.Value.Should().Be(input);
    }
}
