using HE.Investment.AHP.Domain.Scheme.Constants;
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
        var fundingOperationResult = OperationResult.New();
        var housesOperationResult = OperationResult.New();

        var requiredFundingName = "total funding you require";
        RequiredFunding = NumericValidator
            .For(requiredFundingGbp, nameof(RequiredFunding), requiredFundingName, fundingOperationResult)
            .IsProvided()
            .IsNumber()
            .IsWholeNumber()
            .IsBetween(1, 99999999999);

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
