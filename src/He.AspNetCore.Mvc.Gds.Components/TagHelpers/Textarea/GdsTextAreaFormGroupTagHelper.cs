using System.Linq;
using System.Text.Encodings.Web;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using He.AspNetCore.Mvc.Gds.Components.Interfaces;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.Validation;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Textarea
{
    /// <summary>
    /// Adds a gds text are control.
    /// Provides both additional attributes for the current input and also wrapper HTML for:
    /// i) div label, ii) validator span iii) character count iv) hint.
    /// </summary>
    [HtmlTargetElement("gds-textarea", Attributes = InputTagHelperConstants.GdsFormGroupTagHelperForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class GdsTextAreaFormGroupTagHelper : InputTagHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GdsTextAreaFormGroupTagHelper"/> class.
        /// </summary>
        /// <param name="generator">HtmlGenerator.</param>
        public GdsTextAreaFormGroupTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text area is disabled.
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
        /// Gets or sets max words for the optional hint text.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.MaxWordAttributeName)]
        public string MaxWords { get; set; }

        /// <summary>
        /// Gets or sets max length for the optional hint text.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.MaxLengthAttributeName)]
        public string MaxLength { get; set; }

        /// <summary>
        /// Gets or sets max threshold for the optional hint text.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.MaxThresholdAttributeName)]
        public string MaxThreshold { get; set; }

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

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagConstruct.RemoveGdsFromTagName(output);
            var fullHtmlFieldName = IGdsFormGroupTagHelper.GetFullHtmlFieldName(this.ViewContext, this.For.Name);
            var propertyInError = IGdsFormGroupTagHelper.IsPropertyInError(this.ViewContext, fullHtmlFieldName);

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
                false,
                true,
                false,
                this.LegendClasses);

            TagConstruct.ConstructClass(output, $"{CssConstants.GovUkInput}");

            if (this.For.Metadata.ValidatorMetadata.OfType<NumericValidationAttribute>().Any())
            {
                // Assumption is that all 'number' style properties will have the 'NumericValidationAttribute' associated with them.
                TagConstruct.ConstructGenericAttribute(output, HtmlAttributes.InputMode, HtmlAttributes.InputTypes.Numeric);
                TagConstruct.ConstructGenericAttribute(output, HtmlAttributes.Pattern, "[0-9]*");
            }

            if (this.Autocomplete.IsNotNullOrEmpty())
            {
                TagConstruct.ConstructGenericAttribute(output, HtmlAttributes.Autocomplete, this.Autocomplete);
            }

            if (propertyInError.isPropertyInError)
            {
                // Applies GDS error class to the invalid <input>
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkInputError}");
                output.RemoveClass(CssConstants.InputValidationError, HtmlEncoder.Default);
            }

            base.Process(context, output);

            // Removing maxlength in lieu of being able to disable HTML5 attributes
            // https://github.com/dotnet/aspnetcore/issues/18607
            if (output != null && output.Attributes.ContainsName(HtmlAttributes.MaxLength))
            {
                output.Attributes.RemoveAt(output.Attributes.IndexOfName(HtmlAttributes.MaxLength));
            }
        }
    }
}
