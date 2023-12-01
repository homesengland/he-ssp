using System.Globalization;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Extensions;
using Newtonsoft.Json.Linq;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ExpectedCosts : ValueObject
{
    public ExpectedCosts(string? worksCosts, string? onCosts)
    {
        SetValues(worksCosts, onCosts).CheckErrors();
    }

    public int? WorksCosts { get; private set; }

    public int? OnCosts { get; private set; }

    public static ExpectedCosts From(decimal? worksCosts, decimal? onCosts)
    {
        return new ExpectedCosts(worksCosts.ToWholeNumberString(), onCosts.ToWholeNumberString());
    }

    public void CheckErrors()
    {
        SetValues(WorksCosts?.ToString(CultureInfo.InvariantCulture), OnCosts?.ToString(CultureInfo.InvariantCulture), false).CheckErrors();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return WorksCosts ?? null!;
        yield return OnCosts ?? null!;
    }

    private OperationResult SetValues(string? worksCosts, string? onCosts, bool allowNulls = true)
    {
        var operationResult = OperationResult.New();

        WorksCosts = NumericValidator
            .For(worksCosts, FinancialDetailsValidationFieldNames.ExpectedWorksCosts, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.InvalidExpectedWorksCosts)
            .IsBetween(1, 999999999, FinancialDetailsValidationErrors.InvalidExpectedWorksCosts)
            .IsConditionallyRequired(!allowNulls);

        OnCosts = NumericValidator
            .For(onCosts, FinancialDetailsValidationFieldNames.ExpectedOnCosts, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.InvalidExpectedOnCosts)
            .IsBetween(1, 999999999, FinancialDetailsValidationErrors.InvalidExpectedOnCosts)
            .IsConditionallyRequired(!allowNulls);

        return operationResult;
    }
}
