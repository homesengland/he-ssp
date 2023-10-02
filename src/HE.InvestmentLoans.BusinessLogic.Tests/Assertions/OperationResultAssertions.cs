using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using FluentAssertions.Specialized;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
public static class OperationResultAssertions
{
    public static GenericCollectionAssertions<ErrorItem> ContainsOnlyOneErrorMessage(
        this GenericCollectionAssertions<ErrorItem> operationResultAssertions, string errorMessage)
    {
        operationResultAssertions.ContainSingle(x => x.ErrorMessage == errorMessage);
        return operationResultAssertions;
    }
}
