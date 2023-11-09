using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsLandValueModel : FinancialDetailsBaseModel
{
    public FinancialDetailsLandValueModel()
        : base()
    {
    }

    public FinancialDetailsLandValueModel(Guid applicationId, string applicationName, string landValue, string isOnPublicLand)
        : base(applicationId, applicationName)
    {
        LandValue = landValue;
        IsOnPublicLand = isOnPublicLand;
    }

    public string LandValue { get; set; }

    public string IsOnPublicLand { get; set; }
}
