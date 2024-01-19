using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Interfaces;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Radios
{
    /// <summary>
    /// Input tag helper for radio button groups.
    /// </summary>
    [HtmlTargetElement("gds-radio", Attributes = InputTagHelperConstants.GdsFormGroupTagHelperForAttributeName,
        TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("gds-input")]
    public class GdsRadioFormGroupTagHelper : InputTagHelper, IGdsFormGroupTagHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GdsRadioFormGroupTagHelper"/> class.
        /// </summary>
        /// <param name="generator">HtmlGenerator.</param>
        public GdsRadioFormGroupTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the input is disabled.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.DisabledAttributeName)]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is optional hidden text for the associated label.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.HiddenLabelTextAttributeName)]
        public string HiddenLabelText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are additional classes to apply to the associated label.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.LabelClassAttributeName)]
        public string LabelClasses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are additional classes to apply to the associated label.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.LegendClassAttributeName)]
        public string LegendClasses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are additional classes to apply to the associated label.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.LabelTextAttributeName)]
        public string LabelText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the field level validation message.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.ExcludeFieldValidationAttributeName)]
        public bool IsExcludeFieldValidation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is optional hint text.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.HintTextAttributeName)]
        public string HintText { get; set; }

        /// <summary>
        /// Gets or sets the values of the SelectListItems.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.SelectListItemsAttributeName)]
        public IEnumerable<SelectListItem> SelectListItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is an inline radio group.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.InlineRadioGroupAttributeName)]
        public bool IsInlineRadioGroup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is an inline radio group.
        /// </summary>
        [HtmlAttributeName(RadioTagHelperConstants.ConditionalChildInputAttributeName)]
        public bool HasConditionalChildInput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is additional Id Attribute text to add to the end of the Id.
        /// </summary>
        [HtmlAttributeName(RadioTagHelperConstants.ConditionalChildInputOrderAttributeName)]
        public int ConditionalChildInputOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is additional Id Attribute text to add to the end of the Id.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.IsExcludeHeader)]
        public bool IsExcludeHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the index of the option to add text before.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.AddTextAtIndex)]
        public int? AddTextAtIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating what text to add at the index in <see cref="AddTextAtIndex"/>.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.AddTextAtIndexText)]
        public string AddTextAtIndexText { get; set; }

        /// <summary>
        /// Gets or sets the ModelExpression to access the model property.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.GdsFormGroupTagHelperForAttributeName)]
        public ModelExpression AspGovFor
        {
            get => this.For;
            set => this.For = value;
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            TagConstruct.RemoveGdsFromTagName(output);
            var fullHtmlFieldName = IGdsFormGroupTagHelper.GetFullHtmlFieldName(this.ViewContext, this.For.Name);
            var propertyInError = IGdsFormGroupTagHelper.IsPropertyInError(this.ViewContext, fullHtmlFieldName);

            if (this.HasConditionalChildInput)
            {
                // There is a child input defined in the Razor to render the 'conditional reveal' input field. We will manually add this in to the correct location later.
                output?.SuppressOutput();
            }

            var displayText = this.AspGovFor.Metadata.DisplayName;
            if (!string.IsNullOrEmpty(this.LabelText))
            {
                displayText += this.LabelText;
            }

            IGdsFormGroupTagHelper.GenerateElementWrapperContent(
                output,
                fullHtmlFieldName,
                propertyInError,
                this.IsDisabled,
                this.HiddenLabelText,
                this.IsExcludeFieldValidation,
                this.HintText,
                this.LabelClasses,
                displayText,
                null,
                null,
                true,
                false,
                this.IsExcludeHeader,
                false,
                false,
                this.LegendClasses);

            if (this.IsInlineRadioGroup)
            {
                TagConstruct.ConstructClass(output, CssConstants.GovUkRadiosInline);
            }
            else
            {
                TagConstruct.ConstructClass(output, CssConstants.GovUkRadios);

                if (this.HasConditionalChildInput)
                {
                    TagConstruct.ConstructGenericAttribute(output, HtmlAttributes.DataModule, CssConstants.GovUkRadios);
                }
            }

            if (output != null)
            {
                output.TagName = HtmlConstants.Div;

                if (propertyInError.isPropertyInError)
                {
                    // Applies GDS error class to the invalid field
                    TagConstruct.ConstructClass(output, CssConstants.GovUkRadiosError);
                    output.RemoveClass(CssConstants.InputValidationError, HtmlEncoder.Default);
                }

                if (this.SelectListItems?.Any() == true)
                {
                    var realModelType = this.For.ModelExplorer.ModelType;
                    var allowMultiple = typeof(string) != realModelType &&
                                        typeof(IEnumerable).IsAssignableFrom(realModelType);

                    var currentValues =
                        this.Generator.GetCurrentValues(this.ViewContext, this.For.ModelExplorer, this.For.Name, allowMultiple);

                    var counter = 0;
                    foreach (var selectListItem in this.SelectListItems)
                    {
                        var isConditionalRevealInputOption =
                            this.HasConditionalChildInput && counter == this.ConditionalChildInputOrder;

                        output.PostContent.AppendHtml(await this.GenerateContentForOption(
                                fullHtmlFieldName, counter++, selectListItem, currentValues, isConditionalRevealInputOption, output)
                            .ConfigureAwait(false));
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new radio button option.
        /// </summary>
        /// <param name="fullHtmlFieldName">The field name.</param>
        /// <param name="idPostfix">Post fix string for the id of input.</param>
        /// <param name="option">The option radio button to add.</param>
        /// <param name="currentValues">Currently selected values.</param>
        /// <param name="isConditionalRevealInputOption">If this option has a conditionally revealed child input field.</param>
        /// <param name="output">The TagHelperOutput.</param>
        /// <returns>String represent html to add.</returns>
        [SuppressMessage("Performance", "CA1822:Mark members as static",
            Justification = "Don't want to make static")]
        private async Task<string> GenerateContentForOption(
            string fullHtmlFieldName,
            int idPostfix,
            SelectListItem option,
            IEnumerable<string> currentValues,
            bool isConditionalRevealInputOption,
            TagHelperOutput output)
        {
            var elementInputBuilder = new StringBuilder();

            if (this.AddTextAtIndex.HasValue && this.AddTextAtIndex.Value == idPostfix)
            {
                elementInputBuilder.AppendLine($"<div class='govuk-label govuk-!-margin-2'>{this.AddTextAtIndexText}</div>");
            }

            elementInputBuilder.AppendLine($"<div class='{CssConstants.GovUkRadiosItem}'>");

            var inputId = idPostfix == 0 ? fullHtmlFieldName : $"{fullHtmlFieldName}-{idPostfix}";
            elementInputBuilder.AppendLine(
                $"<input id='{inputId}' type='radio' class='{CssConstants.GovUkRadiosInput}' value='{option.Value}' name='{fullHtmlFieldName}' " +
                $"{(currentValues?.Contains(option.Value) == true ? "checked" : string.Empty)} {(isConditionalRevealInputOption ? $"data-aria-controls='conditional-children-{idPostfix}'" : string.Empty)}></input>");

            elementInputBuilder.AppendLine(
                $"<label class='{CssConstants.GovUkLabel} {CssConstants.GovUkRadiosLabel}' for='{inputId}'>{option.Text}</label>");

            elementInputBuilder.AppendLine("</div>");

            if (isConditionalRevealInputOption)
            {
                // There is a child input defined in the Razor to render the 'conditional reveal' input field. Render it in the correct position.
                elementInputBuilder.AppendLine(
                    $"<div class='{CssConstants.GovUkRadiosConditional} {CssConstants.GovUkRadiosConditionalHidden}' id='conditional-children-{idPostfix}'>");
                elementInputBuilder.AppendLine((await output.GetChildContentAsync().ConfigureAwait(false)).GetContent());
                elementInputBuilder.AppendLine("</div>");
            }

            return elementInputBuilder.ToString();
        }
    }
}
