using System.Globalization;
using System.Linq;
using System.Text.Encodings.Web;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Enums;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using He.AspNetCore.Mvc.Gds.Components.Interfaces;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.Validation;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Input
{
    /// <summary>
    /// Extend existing behaviour of *asp-for*
    /// Provides both additional attributes for the current input and also wrapper HTML for:
    /// i) form-group div, ii) validator span iii) label and optional hidden label text.
    /// </summary>
    [HtmlTargetElement("gds-input", Attributes = InputTagHelperConstants.GdsFormGroupTagHelperForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class GdsInputFormGroupTagHelper : InputTagHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GdsInputFormGroupTagHelper"/> class.
        /// </summary>
        /// <param name="generator">HtmlGenerator.</param>
        public GdsInputFormGroupTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the input is disabled.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.DisabledAttributeName)]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input should have an autocomplete attribute.
        /// Current convention is that this has the same value as the property name.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.AutocompleteAttributeName)]
        public string Autocomplete { get; set; }

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
        /// Gets or sets a value indicated whether there is optional prefix text.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.PrefixTextAttributeName)]
        public GdsInputPrefixText? PrefixText { get; set; }

        /// <summary>
        /// Gets or sets a value indicated whether there is is optional suffix text.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.SuffixTextAttributeName)]
        public GdsInputSuffixText? SuffixText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the field level validation message.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.ExcludeLabelAttributeName)]
        public bool IsExcludeLabel { get; set; }

        /// <summary>
        /// Gets or sets the ModelExpression to access the model property.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.GdsFormGroupTagHelperForAttributeName)]
        public ModelExpression AspGovFor
        {
            get => this.For;
            set => this.For = value;
        }

        public bool IsInvalid{ get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagConstruct.RemoveGdsFromTagName(output);

            var fullHtmlFieldName = TagConstruct.RemoveSymbolsHtmFieldName(IGdsFormGroupTagHelper.GetFullHtmlFieldName(this.ViewContext, this.For.Name));
            var name = IGdsFormGroupTagHelper.GetFullHtmlFieldName(this.ViewContext, this.For.Name);

            var propertyInError = IGdsFormGroupTagHelper.IsPropertyInError(this.ViewContext, name);

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
                this.PrefixText,
                this.SuffixText,
                false,
                this.IsExcludeLabel,
                false,
                false,
                false,
                this.LegendClasses);

            if(IsInvalid)
            {
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkInput} {CssConstants.GovUkInput}--error");
            }
            else
            {
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkInput}");
            }

            if (this.For.Metadata.ValidatorMetadata.OfType<NumericValidationAttribute>().Any())
            {
                // Assumption is that all 'number' style properties will have the 'NumericValidationAttribute' associated with them.
                TagConstruct.ConstructGenericAttribute(output, "inputmode", "numeric");
                TagConstruct.ConstructGenericAttribute(output, "pattern", "[0-9]*");
            }

            if (this.Autocomplete.IsNotNullOrEmpty())
            {
                TagConstruct.ConstructGenericAttribute(output, "autocomplete", this.Autocomplete);
            }

            if (propertyInError.isPropertyInError)
            {
                // Applies GDS error class to the invalid <input>
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkInputError}");
                TagConstruct.ConstructGenericAttribute(output, "aria-describedby", $"{fullHtmlFieldName}-error");
            }

            base.Process(context, output);

            if (propertyInError.isPropertyInError)
            {
                output.RemoveClass("input-validation-error", HtmlEncoder.Default);
            }

            // Removing maxlength in lieu of being able to disable HTML5 attributes
            // https://github.com/dotnet/aspnetcore/issues/18607
            if (output != null && output.Attributes.ContainsName("maxlength"))
            {
                output.Attributes.RemoveAt(output.Attributes.IndexOfName("maxlength"));
            }

            if (this.HintText.IsNotNullOrEmpty())
            {
                TagConstruct.ConstructGenericAttribute(output, "aria-describedby", $"{fullHtmlFieldName}-hint");
            }
        }
    }
}
