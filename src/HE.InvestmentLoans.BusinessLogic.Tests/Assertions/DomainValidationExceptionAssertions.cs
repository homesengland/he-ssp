using FluentAssertions;
using FluentAssertions.Specialized;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Assertions;

public static class DomainValidationExceptionAssertions
{
    public static ExceptionAssertions<DomainValidationException> WithOnlyOneErrorMessage(
        this ExceptionAssertions<DomainValidationException> exceptionAssertions, string errorMessage)
    {
        exceptionAssertions.Which.OperationResult.Errors.Should().ContainSingle(x => x.ErrorMessage == errorMessage);
        return exceptionAssertions;
    }
}
