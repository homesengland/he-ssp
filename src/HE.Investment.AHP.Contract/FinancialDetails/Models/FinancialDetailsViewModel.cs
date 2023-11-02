using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;

namespace HE.Investment.AHP.Contract.FinancialDetails.Models;
public class FinancialDetailsViewModel
{
    public FinancialDetailsViewModel()
    {
    }

    public FinancialDetailsViewModel(
        FinancialDetailsId financialDetailsId,
        string? financialSchemaName,
        bool? isPurchasePriceKnown,
        string? purchasePrice,
        string? isSchemaOnPublicLand)
    {
        FinancialDetailsId = financialDetailsId;
        FinancialSchemaName = financialSchemaName;
        IsPurchasePriceKnown = isPurchasePriceKnown;
        PurchasePrice = purchasePrice;
        IsSchemaOnPublicLand = isSchemaOnPublicLand;
    }

    public FinancialDetailsId FinancialDetailsId { get; set; }

    public string? FinancialSchemaName { get; set; }

    public bool? IsPurchasePriceKnown { get; set; }

    public string? PurchasePrice { get; set; }

    public string? IsSchemaOnPublicLand { get; set; }
}
