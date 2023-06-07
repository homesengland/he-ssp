using He.AspNetCore.Mvc.Gds.Components.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;

namespace He.AspNetCore.Mvc.Gds.Components.Extensions
{
    internal static class ViewContextExtensions
    {
        public static (bool hasError, string errorMessage) GetErrorFrom(this ViewContext viewContext, ModelExpression forProperty)
        {
            var fullHtmlFieldName = IGdsFormGroupTagHelper.GetFullHtmlFieldName(viewContext, forProperty.Name);
            var propertyInError = IGdsFormGroupTagHelper.IsPropertyInError(viewContext, fullHtmlFieldName);

            if (!propertyInError.isPropertyInError)
                return (propertyInError.isPropertyInError, null);

            return (propertyInError.isPropertyInError, propertyInError.entry?.Errors.First().ErrorMessage);
        }
    }
}
