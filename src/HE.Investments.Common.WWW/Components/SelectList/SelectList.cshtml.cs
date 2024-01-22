using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.SelectList;

public class SelectList : ViewComponent
{
    public IViewComponentResult Invoke(PaginationResult<SelectListItemViewModel> items, string pagingNavigationUrl, string? addActionUrl = null, string? addActionText = null)
    {
        return View("SelectList", (items, pagingNavigationUrl, addActionUrl, addActionText));
    }
}

public record SelectListItemViewModel(string Url, string Text, string? Description);
