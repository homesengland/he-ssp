using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Specialized;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
internal static class NotFoundAssertions
{
    public static ExceptionAssertions<NotFoundException> ForEntity(
        this ExceptionAssertions<NotFoundException> exceptionAssertions, string entityName)
    {
        exceptionAssertions.Which.EntityName.Should().Be(entityName);

        return exceptionAssertions;
    }
}
