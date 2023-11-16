using System.Globalization;
using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Interfaces;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.Components.TextArea;

public class HeTextAreaTagHelper : InputTagHelper
{
    private const string TextAreaClassName = "govuk-textarea";

    public HeTextAreaTagHelper(IHtmlGenerator generator)
        : base(generator)
    {
    }

    public string? Id { get; set; }

    public string? Text { get; set; }

    public string? Rows { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput? output)
    {
        if (output != null)
        {
            output.TagName = HtmlConstants.Textarea;
            TagConstruct.ConstructId(output, Id);

            if (For != null)
            {
                var contentBuilder = new StringBuilder();

                var fullHtmlFieldName = IGdsFormGroupTagHelper.GetFullHtmlFieldName(ViewContext, For.Name);
                var (isPropertyInError, entry) = IGdsFormGroupTagHelper.IsPropertyInError(ViewContext, fullHtmlFieldName);

                if (isPropertyInError)
                {
                    output.TagName = HtmlConstants.Div;
                    TagConstruct.ConstructClass(output, $"{CssConstants.GovUkFormGroup} {CssConstants.GovUkFormGroupError}");

                    contentBuilder.Append(
                        CultureInfo.InvariantCulture,
                        $"<p id=\"contact-by-email-error\" class=\"govuk-error-message\"><span class=\"govuk-visually-hidden\">Error:</span> {entry.Errors.First().ErrorMessage}</p>");

                    contentBuilder.Append(
                        CultureInfo.InvariantCulture,
                        $"<textarea class=\"govuk-textarea govuk-textarea--error\" rows=\"{Rows ?? "5"}\" id=\"{For.Name}\" name=\"{For.Name}\">{For.Model}</textarea>");

                    output.Content.SetHtmlContent(contentBuilder.ToString());

                    return;
                }

                TagConstruct.ConstructName(output, For.Name);
                TagConstruct.ConstructId(output, For.Name);
                TagConstruct.ConstructClass(output, TextAreaClassName);

                TagConstruct.ConstructGeneric(output, HtmlConstants.Rows, Rows);

                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, (string)For.Model));
            }
            else
            {
                TagConstruct.ConstructName(output, Name);
                TagConstruct.ConstructValue(output, Value);
                TagConstruct.ConstructClass(output, TextAreaClassName);
                TagConstruct.ConstructGeneric(output, HtmlConstants.Rows, Rows);

                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, Text));
            }
        }
    }
}
