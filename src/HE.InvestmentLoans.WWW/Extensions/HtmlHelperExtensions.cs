using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.InvestmentLoans.WWW.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static Task<IHtmlContent> YesNoConditionalInputScript(this IHtmlHelper html, string propertyName)
        {
            return html.RadioConditionalInputScript(
                    ($"{propertyName}", $"{propertyName}-conditional"),
                    ($"{propertyName}-1", $"{propertyName}-1-conditional"));
        }

        public static Task<IHtmlContent> RadioConditionalInputScriptFor(this IHtmlHelper html, string propertyName, int optionsCount)
        {
            var radioConditionalInutIds = new List<(string, string)>();

            for (int i = 0; i < optionsCount; i++)
            {
                if (i == 0)
                    radioConditionalInutIds.Add((propertyName, $"{propertyName}-conditional"));
                else
                    radioConditionalInutIds.Add(($"{propertyName}-{i}", $"{propertyName}-{i}-conditional"));
            }

            return html.PartialAsync("_RadiosWithConditionalInputScript", new { Radios = radioConditionalInutIds });
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
