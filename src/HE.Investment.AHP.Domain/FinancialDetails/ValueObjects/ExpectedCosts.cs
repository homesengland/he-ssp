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

        WorksCosts = CheckNullableIntValue(
            worksCosts,
            FinancialDetailsValidationFieldNames.ExpectedWorksCosts,
            FinancialDetailsValidationErrors.InvalidExpectedWorksCosts,
            allowNulls,
            operationResult);

        OnCosts = CheckNullableIntValue(
            onCosts,
            FinancialDetailsValidationFieldNames.ExpectedOnCosts,
            FinancialDetailsValidationErrors.InvalidExpectedOnCosts,
            allowNulls,
            operationResult);

        return operationResult;
    }

    private int? CheckNullableIntValue(string? value, string fieldName, string errorMsg, bool allowNull, OperationResult operationResult)
    {
        return value.TryParseNullableIntAndValidate(fieldName, errorMsg, allowNull, 0, 999999999, operationResult);
    }
}
