using HE.Investments.Loans.WWW.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Loans.WWW.Extensions;

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

        for (var i = 0; i < optionsCount; i++)
        {
            if (i == 0)
            {
                radioConditionalInutIds.Add((propertyName, $"{propertyName}-conditional"));
            }
            else
            {
                radioConditionalInutIds.Add(($"{propertyName}-{i}", $"{propertyName}-{i}-conditional"));
            }
        }

        return html.PartialAsync("_RadiosWithConditionalInputScript", new ConditionalModel { Radios = radioConditionalInutIds });
    }

    public static Task<IHtmlContent> RadioConditionalInputScript(this IHtmlHelper html, params (string RadioId, string ConditionalInputId)[] radioConditionalInutIds)
    {
        return html.PartialAsync("_RadiosWithConditionalInputScript", new ConditionalModel { Radios = radioConditionalInutIds });
    }

    public static Task<IHtmlContent> CheckboxConditionalInputScript(this IHtmlHelper html, params (string CheckboxId, string ConditionalInputId)[] checkboxConditionalInutIds)
    {
        return html.PartialAsync("_CheckboxesWithConditionalInputScript", new ConditionalModel { Checkboxes = checkboxConditionalInutIds });
    }
}
