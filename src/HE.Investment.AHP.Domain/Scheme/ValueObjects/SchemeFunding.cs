using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SchemeFunding : ValueObject
{
    public SchemeFunding(string? requiredFunding, string? housesToDeliver)
    {
        Build(requiredFunding, housesToDeliver).CheckErrors();
    }

    public SchemeFunding(int? requiredFunding, int? housesToDeliver)
    {
        Build(requiredFunding?.ToString(CultureInfo.InvariantCulture), housesToDeliver?.ToString(CultureInfo.InvariantCulture));
    }

    public int? RequiredFunding { get; private set; }

    public int? HousesToDeliver { get; private set; }

    public void CheckIsComplete()
    {
        Build(RequiredFunding.ToString(), HousesToDeliver.ToString()).CheckErrors();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return RequiredFunding;
        yield return HousesToDeliver;
    }

    private OperationResult Build(string? requiredFundingGbp, string? housesToDeliver)
    {
        var fundingOperationResult = OperationResult.New();
        var housesOperationResult = OperationResult.New();

        var requiredFundingName = "total of funding you are requesting";
        RequiredFunding = NumericValidator
            .For(requiredFundingGbp, nameof(RequiredFunding), requiredFundingName, fundingOperationResult)
            .IsProvided()
            .IsNumber()
            .IsWholeNumber()
            .IsBetween(1, 999999999, "The total funding you require must be 9 digits or less");

        var housesToDeliverName = "number of homes this scheme will deliver";
        HousesToDeliver = NumericValidator
            .For(housesToDeliver, nameof(HousesToDeliver), housesToDeliverName, housesOperationResult)
            .IsProvided()
            .IsNumber()
            .IsWholeNumber()
            .IsBetween(1, 999999);

        return fundingOperationResult.AddValidationErrors(housesOperationResult.Errors);
    }
}
