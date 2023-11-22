using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsLandStatusModel : FinancialDetailsBaseModel
{
    public FinancialDetailsLandStatusModel()
        : base()
    {
    }

    public FinancialDetailsLandStatusModel(Guid applicationId, string applicationName, string actualPurchasePrice, string expectedPurchasePrice, bool isPurchasePriceKnown)
        : base(applicationId, applicationName)
    {
        ActualPurchasePrice = actualPurchasePrice;
        ExpectedPurchasePrice = expectedPurchasePrice;
        IsPurchasePriceKnown = isPurchasePriceKnown;
    }

    public string ActualPurchasePrice { get; set; }

    public string ExpectedPurchasePrice { get; set; }

    public bool IsPurchasePriceKnown { get; set; }
}
