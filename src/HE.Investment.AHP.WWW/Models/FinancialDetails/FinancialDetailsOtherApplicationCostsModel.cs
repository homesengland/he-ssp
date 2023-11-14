using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsOtherApplicationCostsModel : FinancialDetailsBaseModel
{
    public FinancialDetailsOtherApplicationCostsModel()
        : base()
    {
    }

    public FinancialDetailsOtherApplicationCostsModel(Guid applicationId, string applicationName, string expectedWorksCost, string expectedOnCosts)
        : base(applicationId, applicationName)
    {
        ExpectedWorksCosts = expectedWorksCost;
        ExpectedOnCosts = expectedOnCosts;
    }

    public string ExpectedWorksCosts { get; set; }

    public string ExpectedOnCosts { get; set; }
}
