using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using HE.Investments.Common.WWW.TagConstructs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public class HeCustomRadioTagHelper : TagHelper
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public bool Inline { get; set; }

    [HtmlAttributeName("asp-for")]
    public ModelExpression For { get; set; }

    public IEnumerable<SelectListItem> RadioItems { get; set; }

    public IEnumerable<string>? ConditionalInputIds { get; set; }

    public IEnumerable<string>? ConditionalInputLabels { get; set; }

    public IEnumerable<string>? ConditionalInputNames { get; set; }

    public IEnumerable<string>? ConditionalInputValues { get; set; }

    public IEnumerable<string>? RadioHints { get; set; }

    public bool IsConditionalInputInvalid { get; set; }

    public string ConditionalInputError { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput? output)
    {
        if (output != null)
        {
            output.TagName = HtmlConstants.Div;

            var css = CssConstants.GovUkRadios;
            if (Inline)
            {
                css += $" {CssConstants.GovUkRadiosInline}";
            }

            TagConstruct.ConstructClass(output, css);

            var radioItems = RadioItems.ToArray();
            var conditionalInputIds = ConditionalInputIds?.ToArray() ?? Array.Empty<string>();
            var conditionalInputLabels = ConditionalInputLabels?.ToArray() ?? Array.Empty<string>();
            var conditionalInputNames = ConditionalInputNames?.ToArray() ?? Array.Empty<string>();
            var conditionalInputValues = ConditionalInputValues?.ToArray() ?? Array.Empty<string>();
            var hints = RadioHints?.ToArray() ?? Array.Empty<string>();

            var sb = new StringBuilder();
            for (var i = 0; i < RadioItems.Count(); i++)
            {
                var item = radioItems[i];
                var selectRadio = item.Value == Value;
                var text = item.Text;
                var value = item.Value;

                var id = i == 0 ? For.Name : $"{For.Name}-{i}";
                var builder = new RadioBuilder()
                        .AsRadio(id, For.Name, value)
                        .WithLabel(text, id);

                if (conditionalInputLabels.Length > i && conditionalInputValues.Length > i)
                {
                    builder.WithConditionalInput(conditionalInputIds[i], conditionalInputLabels[i], conditionalInputNames[i], conditionalInputValues[i]);
                }
                else if (conditionalInputLabels.Length > i)
                {
                    builder.WithConditionalInput(conditionalInputIds[i], conditionalInputLabels[i], conditionalInputNames[i]);
                }

                if (IsConditionalInputInvalid && selectRadio)
                {
                    builder.WithConditionalErrorMessage(ConditionalInputError);
                }

                if (hints.Length > i)
                {
                    builder.WithHint(hints[i]);
                }

                if (selectRadio)
                {
                    builder.ThatIsChecked();
                }

                sb.Append(builder.Build());
            }

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
