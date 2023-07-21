using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Interfaces;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Select
{
    /// <summary>
    /// Extend existing behaviour of *asp-for*
    /// Provides both additional attributes for the current input and also wrapper HTML for:
    /// i) form-group div, ii) validator span iii) label and optional hidden label text.
    /// </summary>
    [HtmlTargetElement("gds-select", Attributes = InputTagHelperConstants.GdsFormGroupTagHelperForAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GdsSelectFormGroupTagHelper : SelectTagHelper, IGdsFormGroupTagHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GdsSelectFormGroupTagHelper"/> class.
        /// </summary>
        /// <param name="generator">HtmlGenerator.</param>
        public GdsSelectFormGroupTagHelper(IHtmlGenerator generator)
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
        /// This custom property is required as there is a bug in the Net Core SelectTagHelper where it duplicates the items it renders as option tags.
        /// </summary>
        [HtmlAttributeName(InputTagHelperConstants.SelectListItemsAttributeName)]
        public IEnumerable<SelectListItem> SelectListItems { get; set; }

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
            get => For;
            set => For = value;
        }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagConstruct.RemoveGdsFromTagName(output);
            base.Process(context, output);

            var fullHtmlFieldName = TagConstruct.RemoveSymbolsHtmFieldName(IGdsFormGroupTagHelper.GetFullHtmlFieldName(ViewContext, For.Name));
            var name = IGdsFormGroupTagHelper.GetFullHtmlFieldName(ViewContext, For.Name);

            var displayText = AspGovFor.Metadata.DisplayName;
            if (!string.IsNullOrEmpty(LabelText))
            {
                displayText += LabelText;
            }

            var propertyInError = IGdsFormGroupTagHelper.IsPropertyInError(ViewContext, name);

            IGdsFormGroupTagHelper.GenerateElementWrapperContent(
                output,
                fullHtmlFieldName,
                propertyInError,
                IsDisabled,
                HiddenLabelText,
                IsExcludeFieldValidation,
                HintText,
                LabelClasses,
                displayText,
                null,
                null,
                false,
                IsExcludeLabel);

            TagConstruct.ConstructClass(output, $"{CssConstants.GovUkSelect}");

            if (propertyInError.isPropertyInError)
            {
                // Applies GDS error class to the invalid <select>
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkSelectError}");
                output.RemoveClass(CssConstants.InputValidationError, HtmlEncoder.Default);
            }

            if (SelectListItems?.Any() == true)
            {
                // This code is taken from the .Net Core SelectTagHelper.cs: https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.TagHelpers/src/SelectTagHelper.cs
                var realModelType = For.ModelExplorer.ModelType;
                var allowMultiple = typeof(string) != realModelType &&
                                 typeof(IEnumerable).IsAssignableFrom(realModelType);
                var currentValues = Generator.GetCurrentValues(ViewContext, For.ModelExplorer, For.Name, allowMultiple);

                foreach (var selectListItem in SelectListItems)
                {
                    output?.PostContent.AppendHtml(
                        $"<option value='{selectListItem.Value}' {(currentValues?.Contains(selectListItem.Value) == true ? "selected='selected'" : string.Empty)}'>{selectListItem.Text}</option>");
                }
            }
        }
    }
}
