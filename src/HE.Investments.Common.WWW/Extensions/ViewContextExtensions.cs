using He.AspNetCore.Mvc.Gds.Components.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Extensions;

internal static class ViewContextExtensions
{
    public static (bool HasError, string? ErrorMessage) GetErrorFrom(this ViewContext viewContext, ModelExpression forProperty)
    {
        var fullHtmlFieldName = IGdsFormGroupTagHelper.GetFullHtmlFieldName(viewContext, forProperty.Name);
        var (isPropertyInError, entry) = IGdsFormGroupTagHelper.IsPropertyInError(viewContext, fullHtmlFieldName);
        if (!isPropertyInError)
        {
            return (isPropertyInError, null);
        }

        return (isPropertyInError, entry?.Errors.First().ErrorMessage);
    }
}
