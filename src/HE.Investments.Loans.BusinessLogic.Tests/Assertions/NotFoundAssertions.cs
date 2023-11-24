using FluentAssertions.Specialized;
using HE.Investments.Loans.Common.Exceptions;

namespace HE.Investments.Loans.BusinessLogic.Tests.Assertions;
internal static class NotFoundAssertions
{
    public static ExceptionAssertions<NotFoundException> ForEntity(
        this ExceptionAssertions<NotFoundException> exceptionAssertions, string entityName)
    {
        exceptionAssertions.Which.EntityName.Should().Be(entityName);

        return exceptionAssertions;
    }
}
