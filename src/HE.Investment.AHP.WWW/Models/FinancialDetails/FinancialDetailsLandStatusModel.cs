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

    public FinancialDetailsLandStatusModel(Guid applicationId, string applicationName, string purchasePrice, bool isPurchasePriceKnown)
        : base(applicationId, applicationName)
    {
        PurchasePrice = purchasePrice;
        IsPurchasePriceKnown = isPurchasePriceKnown;
    }

    public string PurchasePrice { get; set; }

    public bool IsPurchasePriceKnown { get; set; }
}
