using FluentAssertions;
using FluentAssertions.Specialized;
using HE.Investments.Loans.Common.Exceptions;

namespace HE.Investments.Loans.Common.Tests.Extensions.FluentAssertionsExtensions;
public static class ExceptionAssertionsExtensions
{
    public static Task<ExceptionAssertions<TException>> WithErrorCode<TException>(this Task<ExceptionAssertions<TException>> assertions, string errorCode)
        where TException : DomainException
    {
        return assertions.Where(exception => exception.ErrorCode == errorCode);
    }
}
