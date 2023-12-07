using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SchemeFunding : ValueObject
{
    public SchemeFunding(string? requiredFunding, string? housesToDeliver)
    {
        Build(requiredFunding, housesToDeliver).CheckErrors();
    }

    public decimal? RequiredFunding { get; private set; }

    public int? HousesToDeliver { get; private set; }

    public void CheckIsComplete()
    {
        Build(RequiredFunding.ToWholeNumberString(), HousesToDeliver.ToString()).CheckErrors();
    }

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
            .IsWholeNumber(ValidationErrorMessage.MustBeNumber("total funding you require"))
            .IsBetween(errorMessage: ValidationErrorMessage.StringLengthExceeded("total funding you require", 11));

        HousesToDeliver = NumericValidator
            .For(housesToDeliver, nameof(HousesToDeliver), operationResult)
            .IsProvided("The number of homes this scheme will deliver must be a whole number above 0")
            .IsWholeNumber(ValidationErrorMessage.MustBeNumber("number of homes this scheme will deliver"))
            .IsBetween(errorMessage: ValidationErrorMessage.StringLengthExceeded("number of homes this scheme will deliver", 6));

        return operationResult;
    }
}
