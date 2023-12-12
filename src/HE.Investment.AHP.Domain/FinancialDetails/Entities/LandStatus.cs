using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.Common.Exceptions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class LandStatus : ValueObject, IQuestion
{
    public LandStatus(PurchasePrice? purchasePrice, ExpectedPurchasePrice? expectedPurchasePrice)
    {
        if (purchasePrice.IsProvided() && expectedPurchasePrice.IsProvided())
        {
            throw new DomainException(
                $"{PurchasePrice.Fields.DisplayName} cannot be provided together with {ExpectedPurchasePrice.Fields.DisplayName}",
                CommonErrorCodes.InvalidDomainOperation);
        }

        PurchasePrice = purchasePrice;
        ExpectedPurchasePrice = expectedPurchasePrice;
    }

    public PurchasePrice? PurchasePrice { get; }

    public ExpectedPurchasePrice? ExpectedPurchasePrice { get; }

    public bool IsAnswered()
    {
        return PurchasePrice.IsProvided() || ExpectedPurchasePrice.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return PurchasePrice;
        yield return ExpectedPurchasePrice;
    }
}
