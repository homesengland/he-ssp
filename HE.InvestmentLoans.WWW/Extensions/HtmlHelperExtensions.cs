using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.InvestmentLoans.WWW.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static Task<IHtmlContent> YesNoConditionalInputScript(this IHtmlHelper html, string propertyName)
        {
            return html.RadioConditionalInputScript(
                    ($"{propertyName}Yes", $"{propertyName}Yes-conditional"),
                    ($"{propertyName}No", $"{propertyName}No-conditional"));
        }

        public static Task<IHtmlContent> RadioConditionalInputScript(this IHtmlHelper html, params(string radioId, string conditionalInputId)[] radioConditionalInutIds)
        {
            return html.PartialAsync("_RadiosWithConditionalInputScript", new { Radios = radioConditionalInutIds });
        }

        public static Task<IHtmlContent> CheckboxConditionalInputScript(this IHtmlHelper html, params(string checkboxId, string conditionalInputId)[] checkboxConditionalInutIds)
        {
            return html.PartialAsync("_CheckboxesWithConditionalInputScript", new { Checkboxes = checkboxConditionalInutIds });
        }
    }
}
