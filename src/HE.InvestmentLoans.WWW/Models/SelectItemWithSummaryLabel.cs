using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.InvestmentLoans.WWW.Models;

public class SelectItemWithSummaryLabel : SelectListItem
{
    public string SummaryLabel { get; set; }
}
