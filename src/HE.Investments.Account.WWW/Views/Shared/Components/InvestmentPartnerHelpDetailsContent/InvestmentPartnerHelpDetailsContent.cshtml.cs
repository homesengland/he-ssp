using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Views.Shared.Components.InvestmentPartnerHelpDetailsContent;

public class InvestmentPartnerHelpDetailsContent : ViewComponent
{
    public IViewComponentResult Invoke(string investmentPartnerLink)
    {
        return View("InvestmentPartnerHelpDetailsContent", investmentPartnerLink);
    }
}
