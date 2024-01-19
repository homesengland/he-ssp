using System.Collections;
using System.Collections.Generic;
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

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Checkboxes
{
    /// <summary>
    /// Input tag helper for checkbox button groups.
    /// </summary>
    [HtmlTargetElement("gds-checkbox", Attributes = InputTagHelperConstants.GdsFormGroupTagHelperForAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GdsCheckBoxFormGroupTagHelper : InputTagHelper, IGdsFormGroupTagHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GdsCheckBoxFormGroupTagHelper"/> class.
        /// </summary>
        /// <param name="generator">HtmlGenerator.</param>
        public GdsCheckBoxFormGroupTagHelper(IHtmlGenerator generator)
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
        /// Gets or sets a value indicating whether this is an inline checkbox group.
        /// </summary>
        [HtmlAttributeName(CheckboxsTagHelperConstants.InlineCheckboxGroupAttributeName)]
        public bool IsInlineCheckboxGroup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is an inline checkbox group.
        /// </summary>
        [HtmlAttributeName(CheckboxsTagHelperConstants.ConditionalChildCheckboxsAttributeName)]
        public bool HasConditionalChildCheckboxs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is additional Id Attribute text to add to the end of the Id.
        /// </summary>
        [HtmlAttributeName(CheckboxsTagHelperConstants.ConditionalChildCheckboxsOrderAttributeName)]
        public int ConditionalChildCheckboxsOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is additional Id Attribute text to add to the end of the Id.
        /// </summary>
        [HtmlAttributeName(CheckboxsTagHelperConstants.IsCheckBoxGroupAttributeName)]
        public bool IsCheckBoxGroup { get; set; }

        /// <summary>
        /// Gets or sets a comma denominated integers that reference the child checkbox group.
        /// </summary>
        [HtmlAttributeName(CheckboxsTagHelperConstants.ConditionalPrimaryIndexesAttributeName)]
        public string ConditionalPrimaryIndexes { get; set; }

        /// <summary>
        /// Gets or sets the index for the child check box group.
        /// </summary>
        [HtmlAttributeName(CheckboxsTagHelperConstants.ConditionalChildIndexAttributeName)]
        public string ConditionalChildIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is additional Id Attribute text to add to the end of the Id.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.ExcludeLabelAttributeName)]
        public bool IsExcludeLabel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is additional Id Attribute text to add to the end of the Id.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.IsExcludeHeader)]
        public bool IsExcludeHeader { get; set; }

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

            var fullHtmlFieldName = TagConstruct.RemoveSymbolsHtmFieldName(IGdsFormGroupTagHelper.GetFullHtmlFieldName(this.ViewContext, this.For.Name));
            var name = IGdsFormGroupTagHelper.GetFullHtmlFieldName(this.ViewContext, this.For.Name);

            var propertyInError = IGdsFormGroupTagHelper.IsPropertyInError(this.ViewContext, name);

            output?.Content.SetHtmlContent(string.Empty);

            IGdsFormGroupTagHelper.GenerateElementWrapperContent(
                output,
                fullHtmlFieldName,
                propertyInError,
                this.IsDisabled,
                this.HiddenLabelText,
                this.IsExcludeFieldValidation,
                this.HintText,
                this.LabelClasses,
                this.AspGovFor.Metadata.DisplayName,
                null,
                null,
                false,
                this.IsExcludeLabel,
                this.IsExcludeHeader,
                false,
                this.IsCheckBoxGroup,
                this.LegendClasses);

            if (this.IsInlineCheckboxGroup)
            {
                TagConstruct.ConstructClass(output, CssConstants.GovUkCheckboxesInline);
            }
            else
            {
                if (this.HasConditionalChildCheckboxs)
                {
                    TagConstruct.ConstructGenericAttribute(output, HtmlAttributes.DataModule, CssConstants.GovUkCheckboxs);
                }
            }

            if (output != null)
            {
                output.TagName = HtmlConstants.Div;
                if (propertyInError.isPropertyInError)
                {
                    // Applies GDS error class to the invalid field
                    TagConstruct.ConstructClass(output, CssConstants.GovUkCheckboxsError);
                    output.RemoveClass(CssConstants.InputValidationError, HtmlEncoder.Default);
                }

                if (this.SelectListItems?.Any() == true)
                {
                    var realModelType = this.For.ModelExplorer.ModelType;
                    var allowMultiple = typeof(string) != realModelType &&
                                        typeof(IEnumerable).IsAssignableFrom(realModelType);

                    var currentValues = this.Generator.GetCurrentValues(this.ViewContext, this.For.ModelExplorer, this.For.Name, allowMultiple);

                    var counter = 0;
                    var conditionalPrimaryIndexes = this.ConditionalPrimaryIndexes?.Split(",").Select(int.Parse).ToList();
                    foreach (var selectListItem in this.SelectListItems)
                    {
                        var hasChildContent = conditionalPrimaryIndexes?.Contains(counter);
                        var html = await this.GenerateContentForOption(fullHtmlFieldName, counter == 0 ? null : (int?)counter, selectListItem, hasChildContent ?? false, currentValues, output).ConfigureAwait(false);
                        output.PostContent.AppendHtml(html);
                        counter++;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new checkbox button option.
        /// </summary>
        /// <param name="fullHtmlFieldName">The field name.</param>
        /// <param name="idPostfix">Post fix string for the id of input.</param>
        /// <param name="option">The option radio button to add.</param>
        /// <param name="isConditionalRevealCheckboxsOption">If this option has a conditionally revealed child input field.</param>
        /// <param name="currentValues">Currently selected values.</param>
        /// <param name="output">The TagHelperOutput.</param>
        /// <returns>String represent html to add.</returns>
        private async Task<string> GenerateContentForOption(
            string fullHtmlFieldName,
            int? idPostfix,
            SelectListItem option,
            bool isConditionalRevealCheckboxsOption,
            IEnumerable<string> currentValues,
            TagHelperOutput output)
        {
            var elementInputBuilder = new StringBuilder();

            elementInputBuilder.AppendLine($"<div class='{CssConstants.GovUkCheckboxsItem}'>");

            var id = idPostfix != null ? $"{fullHtmlFieldName}-{idPostfix}" : fullHtmlFieldName;
            var conditionalId = $"conditional-{id}";
            var ariaControls = string.Empty;

            if (isConditionalRevealCheckboxsOption)
            {
                ariaControls = $"aria-controls='{conditionalId}' aria-expanded='false' role='button'";
            }

            elementInputBuilder.AppendLine(
                $"<input id='{id}' type='checkbox' class='{CssConstants.GovUkCheckboxsInput}' value='{option.Value}' name='{fullHtmlFieldName}' " +
                $"{(currentValues?.Contains(option.Value) == true ? "checked" : string.Empty)} {ariaControls}></input>");

            elementInputBuilder.AppendLine(
                $"<label class='{CssConstants.GovUkLabel} {CssConstants.GovUkCheckboxsLabel}' for='{id}'>{option.Text}</label>");

            elementInputBuilder.AppendLine("</div>");

            if (isConditionalRevealCheckboxsOption)
            {
                // There is a child input defined in the Razor to render the 'conditional reveal' input field. Render it in the correct position.
                elementInputBuilder.AppendLine($"<div class='{CssConstants.GovUkCheckboxsConditional} {CssConstants.GovUkcheckboxesConditionalHidden}' id='{conditionalId}'>");
                var outputHtml = (await output.GetChildContentAsync().ConfigureAwait(false)).GetContent();
                elementInputBuilder.AppendLine(outputHtml);
                elementInputBuilder.AppendLine("</div>");
            }

            var element = elementInputBuilder.ToString();
            return element;
        }
    }
}
