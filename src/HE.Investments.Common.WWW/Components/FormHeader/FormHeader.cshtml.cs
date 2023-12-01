using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HE.Investments.Common.WWW.Components.FormHeader;

public class FormHeader : ViewComponent
{
    public IViewComponentResult Invoke(string title, string? caption = null, string? additionalCssClass = null)
    {
        return View("FormHeader", (Caption: caption, Title: title, CssClass: additionalCssClass));
    }
}
