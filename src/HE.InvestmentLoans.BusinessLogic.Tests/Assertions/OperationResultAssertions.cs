using FluentAssertions.Collections;
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
