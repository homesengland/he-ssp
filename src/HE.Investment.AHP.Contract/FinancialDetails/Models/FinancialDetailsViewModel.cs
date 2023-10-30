using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;

namespace HE.Investment.AHP.Contract.FinancialDetails.Models;
public class FinancialDetailsViewModel
{
    public FinancialDetailsViewModel(
        FinancialDetailsId financialDetailsId,
        string? financialSchemaName,
        bool? isPurchasePriceKnown,
        string? purchasePrice)
    {
        FinancialDetailsId = financialDetailsId;
        FinancialSchemaName = financialSchemaName;
        IsPurchasePriceKnown = isPurchasePriceKnown;
        PurchasePrice = purchasePrice;
    }

    public FinancialDetailsId FinancialDetailsId { get; }

    public string? FinancialSchemaName { get; }

    public bool? IsPurchasePriceKnown { get; }

    public string? PurchasePrice { get; }
}
