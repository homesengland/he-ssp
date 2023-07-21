using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Radios
{
    /// <summary>
    /// Class GdsRadioYesNoTagHelper.
    /// Implements the <see cref="TagHelper" />.
    /// </summary>
    /// <seealso cref="TagHelper" />
    public class GdsRadioYesNoTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GdsRadioYesNoTagHelper"/> is inline.
        /// </summary>
        /// <value><c>true</c> if inline; otherwise, <c>false</c>.</value>
        public bool Inline { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public ModelExpression InnerInputFor { get; set; }

        public ModelExpression SecondInnerInputFor { get; set; }

        public string InnerInputValue { get; set; }

        public string SecondInnerInputValue { get; set; }

        public bool WithYesInput { get; set; }

        public bool WithNoInput { get; set; }

        public bool WithFileUpload { get; set; }

        public string FileUploadHint { get; set; }

        public string InnerInputText { get; set; }

        public string SecondInnerInputText { get; set; }

        public bool IsConditionalInputInvalid { get; set; }

        public string ConditionalInputError { get; set; }

        /// <summary>
        /// Synchronously executes the <see cref="TagHelper" /> with the given <paramref name="context" /> and
        /// <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
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
                var texts = CommonResponse.YesNoAnswers();

                var sb = new StringBuilder();
                foreach (var text in texts)
                {
                    if (For is null)
                    {
                        var selectRadio = text == Value;
                        sb.Append(TagConstruct.ConstructRadioInputLabel($"{Id}{text}", Name, text, text, text, selectRadio));
                    }
                    else
                    {
                        var id = text == CommonResponse.Yes ? For.Name : $"{For.Name}-1";
                        var builder = TagConstruct.CreateRadio()
                            .AsRadio(id, For.Name, text)
                            .WithLabel(text, id);

                        if (HasConditionalInput(text))
                        {
                            if (text == CommonResponse.No && IsSecondInnerInputConfigured())
                            {
                                builder.WithConditionalInput(SecondInnerInputFor?.Name, SecondInnerInputText, SecondInnerInputFor?.Name ?? InnerInputFor?.Name, SecondInnerInputValue ?? InnerInputValue);
                            }
                            else
                            {
                                builder.WithConditionalInput(InnerInputFor?.Name, InnerInputText, InnerInputFor?.Name, InnerInputValue);
                            }
                        }

                        if (IsConditionalInputInvalid && IsSelected(text))
                        {
                            builder.WithConditionalErrorMessage(ConditionalInputError);
                        }

                        if (WithFileUpload)
                        {
                            builder.WithFileUpload(FileUploadHint);
                        }

                        if (IsSelected(text))
                        {
                            builder.ThatIsChecked();
                        }

                        sb.Append(builder.Build());
                    }
                }

                output.Content.SetHtmlContent(sb.ToString());
            }
        }
        private bool HasConditionalInput(string text)
        {
            return (text == CommonResponse.Yes && WithYesInput) || (text == CommonResponse.No && WithNoInput);
        }

        private bool IsSecondInnerInputConfigured()
        {
            return !string.IsNullOrEmpty(SecondInnerInputText);
        }

        private bool IsSelected(string text)
        {
            return text == (string)For.Model;
        }
    }
}
