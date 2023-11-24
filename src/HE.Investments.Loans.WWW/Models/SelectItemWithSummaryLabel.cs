using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Loans.WWW.Models;

public class SelectItemWithSummaryLabel : SelectListItem
{
    public string SummaryLabel { get; set; }
}
