using He.AspNetCore.Mvc.Gds.Components.Enums;
using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Components.FormField;

public class FormField : ViewComponent
{
    public IViewComponentResult Invoke(string title, string value, ModelExpression forField, string? fieldName = null, GdsInputPrefixText? prefix = null)
    {
        var (isInvalid, error) = @ViewData.ModelState.GetErrors(fieldName ?? string.Empty);
        return View("FormField", (title, value, fieldName, forField, prefix, isInvalid, error));
    }
}
