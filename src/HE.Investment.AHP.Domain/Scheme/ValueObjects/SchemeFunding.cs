using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SchemeFunding : ValueObject
{
    public SchemeFunding(string? requiredFunding, string? housesToDeliver)
    {
        Build(requiredFunding, housesToDeliver).CheckErrors();
    }

    public long RequiredFunding { get; private set; }

    public int HousesToDeliver { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return RequiredFunding;
        yield return HousesToDeliver;
    }

    private OperationResult Build(string? requiredFundingGbp, string? housesToDeliver)
    {
        var operationResult = OperationResult.New();

        RequiredFunding = NumericValidator
            .For(requiredFundingGbp, nameof(RequiredFunding), operationResult)
            .IsProvided("Enter the total of funding you are requesting")
            .IsWholeNumber("The total funding you require must be a number")
            .IsBetween(errorMessage: "The total funding you require must be 11 digits or less");

        HousesToDeliver = NumericValidator
            .For(housesToDeliver, nameof(HousesToDeliver), operationResult)
            .IsProvided("The number of homes this scheme will deliver must be a whole number above 0")
            .IsWholeNumber("The number of homes this scheme will deliver must be a number")
            .IsBetween(1, 999999, "The number of homes this scheme will deliver must be 6 digits or less");

        return operationResult;
    }
}
