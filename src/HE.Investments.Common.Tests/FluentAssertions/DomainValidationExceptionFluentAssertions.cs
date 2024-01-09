using FluentAssertions;
using FluentAssertions.Specialized;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investments.Common.Tests.FluentAssertions;

public static class DomainValidationExceptionFluentAssertions
{
    public static void WithSingleError(this ExceptionAssertions<DomainValidationException> exception, string errorMessage)
    {
        exception.Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == errorMessage && !string.IsNullOrWhiteSpace(x.AffectedField));
    }
}
