using System.Globalization;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Extensions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class LandValue : ValueObject
{
    public LandValue(string? isLandPublic, string? landValue)
    {
        SetValues(isLandPublic, landValue).CheckErrors();
    }

    public int? Value { get; private set; }

    public bool? IsLandPublic { get; private set; }

    public bool IsAnyValueNotNull => IsLandPublic.HasValue || Value.HasValue;

    public static LandValue From(bool? isLandPublic, decimal? value)
    {
        return new LandValue(isLandPublic.MapToCommonResponse(), value.HasValue ? value.ToWholeNumberString() : null);
    }

    public void CheckErrors()
    {
        SetValues(IsLandPublic.MapToCommonResponse(), Value?.ToString(CultureInfo.InvariantCulture), false).CheckErrors();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value ?? null!;
        yield return IsLandPublic ?? null!;
    }

    private OperationResult SetValues(string? isLandPublic, string? landValue, bool allowNulls = true)
    {
        var operationResult = OperationResult.New();

        Value = NumericValidator
            .For(landValue, FinancialDetailsValidationFieldNames.LandValue, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.InvalidLandValue)
            .IsBetween(1, 999999999, FinancialDetailsValidationErrors.InvalidLandValue)
            .IsConditionallyRequired(!allowNulls);

        IsLandPublic = isLandPublic?.MapToNonNullableBool();

        if (!IsLandPublic.HasValue && !allowNulls)
        {
            operationResult.AddValidationError(
                FinancialDetailsValidationFieldNames.LandOwnership,
                FinancialDetailsValidationErrors.NoLandOwnershipProvided);
        }

        return operationResult;
    }
}
