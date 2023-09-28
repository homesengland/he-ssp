using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Security.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Security.ValueObjects;

public class DirectorLoansSubordinateTests
{
    [Fact]
    public void ShouldCreateDirectorLoansSubordinate()
    {
        // given & when
        var directorLoansSubordinate = new DirectorLoansSubordinate(true, string.Empty);

        // then
        directorLoansSubordinate.CanBeSubordinated.Should().BeTrue();
        directorLoansSubordinate.ReasonWhyCannotBeSubordinated.Should().BeEmpty();
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenReasonWhyCannotBeSubordinatedIsNotProvided()
    {
        // given & when
        var action = () => new DirectorLoansSubordinate(false, string.Empty);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.EnterMoreDetails);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenReasonWhyCannotBeSubordinatedIsTooLong()
    {
        // given & when
        var action = () => new DirectorLoansSubordinate(false, new string('a', MaximumInputLength.LongInput + 1));

        // then
        action.Should()
            .Throw<DomainValidationException>()
            .WithMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.SubordinatedLoans));
    }

    [Fact]
    public void ShouldReasonWhyCannotBeSubordinatedBeEmpty_WhenCanBeSubordinatedIsTrue()
    {
        // given & when
        var directorLoansSubordinate = new DirectorLoansSubordinate(true, "test");

        // then
        directorLoansSubordinate.CanBeSubordinated.Should().BeTrue();
        directorLoansSubordinate.ReasonWhyCannotBeSubordinated.Should().BeEmpty();
    }
}
