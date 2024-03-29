using FluentAssertions.Specialized;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investments.Loans.BusinessLogic.Tests.Assertions;

public static class DomainValidationExceptionAssertions
{
    public static ExceptionAssertions<DomainValidationException> WithOnlyOneErrorMessage(
        this ExceptionAssertions<DomainValidationException> exceptionAssertions, string errorMessage)
    {
        exceptionAssertions.Which.OperationResult.Errors.Should().ContainSingle(x => x.ErrorMessage == errorMessage);
        return exceptionAssertions;
    }

    public static ExceptionAssertions<DomainValidationException> WithErrorMessage(
        this ExceptionAssertions<DomainValidationException> exceptionAssertions, string errorMessage)
    {
        exceptionAssertions.Which.OperationResult.Errors.Should().Contain(x => x.ErrorMessage == errorMessage);
        return exceptionAssertions;
    }
}
