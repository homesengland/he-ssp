using System.Globalization;
using System.Linq;
using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Enums;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace He.AspNetCore.Mvc.Gds.Components.Interfaces
{
    /// <summary>
    /// Interface with default methods to help with generating the standard GDS Html for custom TagHelpers.
    /// </summary>
    public interface IGdsFormGroupTagHelper : ITagHelper
    {
        /// <summary>
        /// Gets or sets a value indicating whether the input is disabled.
        /// </summary>
        bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is optional hidden text for the associated label.
        /// </summary>
        string HiddenLabelText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are additional classes to apply to the associated label.
        /// </summary>
        string LabelClasses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the field level validation message.
        /// </summary>
        bool IsExcludeFieldValidation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is optional hint text.
        /// </summary>
        string HintText { get; set; }

        /// <summary>
        /// Get the full Html field name for the current element from the model property.
        /// </summary>
        /// <param name="viewContext">The ViewContext.</param>
        /// <param name="forName">The 'forName' from the model property.</param>
        /// <returns>The full Html FieldName.</returns>
        public static string GetFullHtmlFieldName(ViewContext viewContext, string forName)
        {
            return viewContext?.ViewData?.TemplateInfo?.GetFullHtmlFieldName(forName);
        }

        /// <summary>
        /// Check if the current model Property is in Error.
        /// </summary>
        /// <param name="viewContext">The ViewContext.</param>
        /// <param name="fullHtmlFieldName">The full Html FieldName.</param>
        /// <returns>Whether the model Property is in Error.</returns>
        public static (bool isPropertyInError, ModelStateEntry entry) IsPropertyInError(ViewContext viewContext, string fullHtmlFieldName)
        {
            ModelStateEntry entry = null;

            var errorResult = viewContext?.ViewData?.ModelState?.TryGetValue(fullHtmlFieldName, out entry);

            if (errorResult == true && entry != null)
            {
                return (entry?.Errors.Count > 0, entry);
            }

            return (false, null);
        }

        /// <summary>
        /// Generate tHE.InvestmentLoans compliant Html that gets wrapped around the current control.
        /// </summary>
        /// <param name="output">the TagHelperOutput.</param>
        /// <param name="fullHtmlFieldName">The full Html FieldName.</param>
        /// <param name="propertyInError">Whether the model Property is in Error.</param>
        /// <param name="isDisabled">Whether the model Property is disabled.</param>
        /// <param name="hiddenLabelText">Optional hidden label text.</param>
        /// <param name="isExcludeFieldValidation">Whether to exclude the Html for individual field level validation.</param>
        /// <param name="hintText">Optional hint text.</param>
        /// <param name="labelClasses">Any additional classes the label might need.</param>
        /// <param name="displayName">The display name from the model property.</param>
        /// <param name="prefixText">Optional prefix text.</param>
        /// <param name="suffixText">Optional suffix text.</param>
        /// <param name="isRadioGroup">Whether input group is a radio button group.</param>
        /// <param name="isExcludeLabel">Whether to exclude the Html for individual Label.</param>
        /// <param name="isExcludeHeader">Whether to exclude the Html for individual Header.</param>
        /// <param name="isTextArea">Whether input is text area control.</param>
        /// <param name="isCheckBoxGroup">Whether input is checkbox group control.</param>
        /// <param name="legendClass">Optional legend class.</param>
        public static void GenerateElementWrapperContent(
            TagHelperOutput output,
            string fullHtmlFieldName,
            (bool isPropertyInError, ModelStateEntry entry) propertyInError,
            bool isDisabled,
            string hiddenLabelText,
            bool isExcludeFieldValidation,
            string hintText,
            string labelClasses,
            string displayName,
            GdsInputPrefixText? prefixText = null,
            GdsInputSuffixText? suffixText = null,
            bool isRadioGroup = false,
            bool isExcludeLabel = false,
            bool isExcludeHeader = false,
            bool isTextArea = false,
            bool isCheckBoxGroup = false,
            string legendClass = null)
        {
            var hasPrefixOrSuffix = suffixText != null || prefixText != null;

            // Most custom Html goes before the element.
            GeneratePreElementContent(
                output,
                fullHtmlFieldName,
                propertyInError,
                isDisabled,
                hiddenLabelText,
                isExcludeFieldValidation,
                hintText,
                labelClasses,
                displayName,
                hasPrefixOrSuffix,
                prefixText,
                isRadioGroup,
                isExcludeLabel,
                isExcludeHeader,
                isTextArea,
                isCheckBoxGroup,
                legendClass);

            // Post-element Html to close out custom tags.
            GeneratePostElementContent(output, hasPrefixOrSuffix, suffixText, isRadioGroup, isCheckBoxGroup);
        }

        /// <summary>
        /// Generate tHE.InvestmentLoans compliant Html that gets prepended to the current control.
        /// </summary>
        /// <param name="output">the TagHelperOutput.</param>
        /// <param name="fullHtmlFieldName">The full Html FieldName.</param>
        /// <param name="propertyInError">Whether the model Property is in Error.</param>
        /// <param name="isDisabled">Whether the model Property is disabled.</param>
        /// <param name="hiddenLabelText">Optional hidden label text.</param>
        /// <param name="isExcludeFieldValidation">Whether to exclude the Html for individual field level validation.</param>
        /// <param name="hintText">Optional hint text.</param>
        /// <param name="labelClasses">Any additional classes the label might need.</param>
        /// <param name="displayName">The display name from the model property.</param>
        /// <param name="hasPrefixOrSuffix">Indicates whether a prefix or suffix is included.</param>
        /// <param name="prefixText">Optional prefix text.</param>
        /// <param name="isRadioGroup">Whether input group is a radio button group.</param>
        /// <param name="isExcludeLabel">Whether to exclude the Html for individual Label.</param>
        /// <param name="isExcludeHeader">Whether to exclude the Html for individual Header.</param>
        /// <param name="isTextArea">Whether input is text area control.</param>
        /// <param name="isCheckBoxGroup">Whether input is checkbox group control.</param>
        /// <param name="legendClass">Optional legend class.</param>
        private static void GeneratePreElementContent(
           TagHelperOutput output,
           string fullHtmlFieldName,
           (bool isPropertyInError, ModelStateEntry entry) propertyInError,
           bool isDisabled,
           string hiddenLabelText,
           bool isExcludeFieldValidation,
           string hintText,
           string labelClasses,
           string displayName,
           bool hasPrefixOrSuffix,
           GdsInputPrefixText? prefixText,
           bool isRadioGroup,
           bool isExcludeLabel,
           bool isExcludeHeader,
           bool isTextArea,
           bool isCheckBoxGroup,
           string legendClass)
        {
            if (isDisabled)
            {
                var disabledAttribute = new TagHelperAttribute("disabled", "disabled");
                output?.Attributes.Add(disabledAttribute);
            }

            if (hiddenLabelText.IsNotNullOrEmpty())
            {
                hiddenLabelText = $"<span class='{CssConstants.GovUkVisuallyHidden}'>{hiddenLabelText}</span>";
            }

            legendClass ??= CssConstants.GovUkFieldSetLegendM;

            var preElementStringBuilder = new StringBuilder();
            preElementStringBuilder.AppendLine($"<div id='{fullHtmlFieldName}-form-group' class='{CssConstants.GovUkFormGroup} {(propertyInError.isPropertyInError ? CssConstants.GovUkFormGroupError : string.Empty)}'>");

            var errorDescribedBy = string.Empty;
            if (propertyInError.isPropertyInError && !isExcludeFieldValidation)
            {
                errorDescribedBy = $"aria-describedby = '{fullHtmlFieldName}-error'";
            }

            if (isRadioGroup)
            {
                preElementStringBuilder.AppendLine($"<fieldset class='{CssConstants.GovUkFieldSet}' {errorDescribedBy}>");
                preElementStringBuilder.AppendLine($"<legend class='{CssConstants.GovUkFieldSetLegend} {legendClass}'>");

                if (!isExcludeHeader)
                {
                    preElementStringBuilder.AppendLine($"<h1 class='{CssConstants.GovUkFieldSetHeading}'>{displayName} {hiddenLabelText}</h1>");
                }

                preElementStringBuilder.AppendLine("</legend>");
            }
            else if (isCheckBoxGroup)
            {
                preElementStringBuilder.AppendLine($"<fieldset class='{CssConstants.GovUkFieldSet}' {errorDescribedBy}>");
                preElementStringBuilder.AppendLine($"<legend class='{CssConstants.GovUkFieldSetLegend} {legendClass}'>");

                if (!isExcludeLabel)
                {
                    preElementStringBuilder.AppendLine($"<label class='{CssConstants.GovUkLabel} {(labelClasses.IsNullOrEmpty() ? string.Empty : labelClasses)}' for='{fullHtmlFieldName}'>{displayName} {hiddenLabelText}</label>");
                }

                if (!isExcludeHeader)
                {
                    preElementStringBuilder.AppendLine($"<h1 class='{CssConstants.GovUkFieldSetHeading}'>{displayName} {hiddenLabelText}</h1>");
                }

                preElementStringBuilder.AppendLine("</legend>");
            }
            else
            {
                if (!isExcludeLabel)
                {
                    preElementStringBuilder.AppendLine($"<label class='{CssConstants.GovUkLabel} {(labelClasses.IsNullOrEmpty() ? string.Empty : labelClasses)}' for='{fullHtmlFieldName}'>{displayName} {hiddenLabelText}</label>");
                }
            }

            if (isTextArea)
            {
            }

            if (hintText.IsNotNullOrEmpty())
            {
                preElementStringBuilder.AppendLine($"<div class='{CssConstants.GovUkHint}' id='{fullHtmlFieldName}-hint'>{hintText}</div>");
            }

            if (!isExcludeFieldValidation)
            {
                var errorMessage = string.Empty;
                if (propertyInError.isPropertyInError)
                {
                    errorMessage = propertyInError.entry?.Errors.First().ErrorMessage;
                }

                if (propertyInError.isPropertyInError)
                {
                    preElementStringBuilder.AppendLine(
                        $"<span id='{fullHtmlFieldName}-error' class='{CssConstants.GovUkErrorMessage} field-validation-error' data-valmsg-for='{fullHtmlFieldName}' data-valmsg-replace='true'><span class='govuk-visually-hidden'>Error:</span>{errorMessage}</span>");
                }
            }

            if (hasPrefixOrSuffix)
            {
                // If we have either prefix or suffix, we need to generate the wrapper.
                preElementStringBuilder.AppendLine($"<div class='{CssConstants.GovUkInputWrapper}'>");
            }

            if (prefixText != null)
            {
                preElementStringBuilder.AppendLine($"<div class='{CssConstants.GovUkInputPrefix}' aria-hidden='true'>{prefixText.GetDisplay()}</div>");
            }

            output?.PreElement.SetHtmlContent(preElementStringBuilder.ToString());
        }

        /// <summary>
        /// Generate tHE.InvestmentLoans compliant Html that gets appended to the current control.
        /// </summary>
        /// <param name="output">the TagHelperOutput.</param>
        /// <param name="hasPrefixOrSuffix">Indicates if  prefix or suffix is included.</param>
        /// <param name="suffixText">Optional suffix text.</param>
        /// <param name="isRadioGroup">Whether input group is a radio button group.</param>
        /// <param name="isCheckBoxGroup">Whether input is checkbox group control.</param>
        private static void GeneratePostElementContent(
            TagHelperOutput output,
            bool hasPrefixOrSuffix,
            GdsInputSuffixText? suffixText,
            bool isRadioGroup,
            bool isCheckBoxGroup)
        {
            var postElementStringBuilder = new StringBuilder();

            if (suffixText != null)
            {
                postElementStringBuilder.AppendLine($"<div class='{CssConstants.GovUkInputSuffix}' aria-hidden='true'>{suffixText.GetDisplay()}</div>");
            }

            if (hasPrefixOrSuffix)
            {
                postElementStringBuilder.AppendLine("</div>");
            }

            if (isRadioGroup)
            {
                postElementStringBuilder.AppendLine("</fieldset>");
            }

            if (isCheckBoxGroup)
            {
                postElementStringBuilder.AppendLine("</fieldset>");
            }

            postElementStringBuilder.AppendLine("</div>");

            output?.PostElement.SetHtmlContent(postElementStringBuilder.ToString());
        }
    }
}
