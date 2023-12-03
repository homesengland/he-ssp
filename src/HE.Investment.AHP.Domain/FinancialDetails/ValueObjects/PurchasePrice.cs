using System.Globalization;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using Newtonsoft.Json.Linq;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class PurchasePrice : ValueObject
{
    public PurchasePrice(string? actualPurchasePrice, string? expectedPurchasePrice)
    {
        if (!string.IsNullOrWhiteSpace(actualPurchasePrice))
        {
            if (!decimal.TryParse(actualPurchasePrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var price))
            {
                OperationResult.New()
                    .AddValidationError(FinancialDetailsValidationFieldNames.PurchasePrice, FinancialDetailsValidationErrors.InvalidActualPurchasePrice)
                    .CheckErrors();
            }

            ActualPrice = price;
        }

        if (!string.IsNullOrWhiteSpace(expectedPurchasePrice))
        {
            if (!decimal.TryParse(expectedPurchasePrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var price))
            {
                OperationResult.New()
                    .AddValidationError(FinancialDetailsValidationFieldNames.PurchasePrice, FinancialDetailsValidationErrors.InvalidExpectedPurchasePrice)
                    .CheckErrors();
            }

            ExpectedPrice = price;
        }
    }

    public decimal? ActualPrice { get; private set; }

    public decimal? ExpectedPrice { get; private set; }

    public bool IsAnyValueNotNull => ActualPrice.HasValue || ExpectedPrice.HasValue;

    public static PurchasePrice From(decimal? actualPurchasePrice, decimal? expectedPurchasePrice)
    {
        return new PurchasePrice(actualPurchasePrice.ToMoneyString(), expectedPurchasePrice.ToMoneyString());
    }

    public void CheckErrors()
    {
        SetValues(ActualPrice, ExpectedPrice, false).CheckErrors();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ActualPrice ?? null!;
        yield return ExpectedPrice ?? null!;
    }

    private OperationResult SetValues(decimal? actualPrice, decimal? expectedPrice, bool allowNulls = true)
    {
        var result = OperationResult.New();
        if (!allowNulls && !actualPrice.HasValue && !expectedPrice.HasValue)
        {
            result.AddValidationError(
                FinancialDetailsValidationFieldNames.PurchasePrice,
                FinancialDetailsValidationErrors.NoPurchasePrice);
        }

        if (actualPrice.HasValue)
        {
            if (actualPrice is < 0 or >= 1000000000)
            {
                result.AddValidationError(
                    FinancialDetailsValidationFieldNames.PurchasePrice, FinancialDetailsValidationErrors.InvalidActualPurchasePrice);
            }
            else
            {
                ActualPrice = actualPrice.Value;
                ExpectedPrice = null;
            }
        }
        else if (expectedPrice.HasValue)
        {
            if (expectedPrice is < 0 or >= 1000000000)
            {
                result.AddValidationError(
                    FinancialDetailsValidationFieldNames.PurchasePrice, FinancialDetailsValidationErrors.InvalidExpectedPurchasePrice);
            }
            else
            {
                ActualPrice = null;
                ExpectedPrice = expectedPrice.Value;
            }
        }

        return result;
    }
}
