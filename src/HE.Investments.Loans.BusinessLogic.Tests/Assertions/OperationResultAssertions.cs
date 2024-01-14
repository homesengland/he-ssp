using FluentAssertions.Collections;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investments.Loans.BusinessLogic.Tests.Assertions;
public static class OperationResultAssertions
{
    public static GenericCollectionAssertions<ErrorItem> ContainsOnlyOneErrorMessage(
        this GenericCollectionAssertions<ErrorItem> operationResultAssertions, string errorMessage)
    {
        operationResultAssertions.ContainSingle(x => x.ErrorMessage == errorMessage);
        return operationResultAssertions;
    }

    public static GenericCollectionAssertions<ErrorItem> ContainsErrorMessages(
        this GenericCollectionAssertions<ErrorItem> operationResultAssertions, params string[] errorMessages)
    {
        foreach (var message in errorMessages)
        {
            operationResultAssertions.Contain(x => x.ErrorMessage == message);
        }

        return operationResultAssertions;
    }
}
