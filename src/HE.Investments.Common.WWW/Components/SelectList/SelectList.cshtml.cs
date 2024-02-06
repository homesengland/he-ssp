using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.SelectList;

public class SelectList : ViewComponent
{
    public IViewComponentResult Invoke(
        PaginationResult<SelectListItemViewModel> items,
        string pagingNavigationUrl,
        string? addActionUrl = null,
        string? addActionText = null,
        ParagraphWithLinkModel? paragraphWithLink = null)
    {
        return View("SelectList", (items, pagingNavigationUrl, addActionUrl, addActionText, paragraphWithLink));
    }
}

public record SelectListItemViewModel(string Url, string Text, string? Description);
