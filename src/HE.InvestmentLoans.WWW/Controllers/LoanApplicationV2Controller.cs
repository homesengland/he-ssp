using HE.InvestmentLoans.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[FeatureGate(LoansFeatureFlags.SaveApplicationDraftInCrm)]
[Route("loan-application")]
[Authorize]
public class LoanApplicationV2Controller : Controller
{
    // GET
    public string Index()
    {
        return "it-works";
    }
}
