using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HE.Investments.Common.WWW.Components.PageHeader;

public class PageHeader : ViewComponent
{
    public IViewComponentResult Invoke(string? title = null, string? caption = null, string? hint = null, string? additionalCssClass = null)
    {
        return View("PageHeader", (Caption: caption, Title: title, Hint: hint, CssClass: additionalCssClass));
    }
}
