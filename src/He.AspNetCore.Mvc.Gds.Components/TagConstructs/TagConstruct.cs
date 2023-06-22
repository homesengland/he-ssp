using System.Linq;
using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Enums;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Radios;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

#pragma warning disable CA1062

namespace He.AspNetCore.Mvc.Gds.Components.TagConstructs
{
    /// <summary>
    /// Class TagConstruct.
    /// </summary>
    public static class TagConstruct
    {
        /// <summary>
        /// Constructs the set HTML.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="text">The text.</param>
        /// <returns>System.String.</returns>
        public static string ConstructSetHtml(TagHelperOutput output, string text = null)
        {
            var innerContent = output?.GetChildContentAsync()?.Result?.GetContent();

            if (innerContent == null)
            {
                return text;
            }

            if (text == null)
            {
                return $"{innerContent}";
            }

            return $"{text} {innerContent}";
        }

        /// <summary>
        /// Construct a class by merging any specified class entries on the element.
        /// </summary>
        /// <param name="output">The TagHelperOutput.</param>
        /// <param name="className">The className.</param>
        public static void ConstructClass(TagHelperOutput output, string className)
        {
            if (output != null)
            {
                var existingClass = output.Attributes.FirstOrDefault(f => f.Name == "class");
                string cssClass;
                if (existingClass != null)
                {
                    output.Attributes.Remove(existingClass);
                    cssClass = existingClass.Value.ToString();
                    cssClass = $"{cssClass} {className}";
                }
                else
                {
                    cssClass = className;
                }

                var taClass = new TagHelperAttribute("class", cssClass);
                output.Attributes.Add(taClass);
            }
        }

        /// <summary>
        /// Modifies the class attribute of a TagHelperOutput object by appending "--error" to the existing class or setting it as a new class if no class attribute exists.
        /// </summary>
        /// <param name="output">The TagHelperOutput object to modify.</param>
        /// <param name="className">The class name to apply.</param>
        public static void ChangeClassToError(TagHelperOutput output, string className)
        {
            if (output != null)
            {
                var taClass = new TagHelperAttribute("class", $"{className}--error");
                output.Attributes.Add(taClass);
            }
        }

        /// <summary>
        /// Construct a class by merging any specified class entries on the element.
        /// </summary>
        /// <param name="output">The TagHelperOutput.</param>
        /// <param name="className">The className.</param>
        public static void ConstructClassForSize(TagHelperOutput output, string className, ControlSize size)
        {
            if (output != null)
            {
                var existingClass = output.Attributes.FirstOrDefault(f => f.Name == "class");
                string cssClass;

                var classWithSize = size switch
                {
                    ControlSize.S => $"{className}--s",
                    ControlSize.M => $"{className}--m",
                    ControlSize.L => $"{className}--l",
                    ControlSize.Xl => $"{className}--xl",
                    _ => throw new System.NotImplementedException(),
                };

                if (existingClass != null)
                {
                    output.Attributes.Remove(existingClass);
                    cssClass = existingClass.Value.ToString();
                    cssClass = $"{cssClass} {classWithSize}";
                }
                else
                {
                    cssClass = classWithSize;
                }

                var taClass = new TagHelperAttribute("class", cssClass);
                output.Attributes.Add(taClass);
            }
        }


        public static void ConstructHeaderClass(TagHelperOutput output, ControlSize size)
        {
            if (output != null)
            {
                var className = size switch
                {
                    ControlSize.S => CssConstants.GovUkHs,
                    ControlSize.M => CssConstants.GovUkHm,
                    ControlSize.L => CssConstants.GovUkHl,
                    ControlSize.Xl => CssConstants.GovUkHxl,
                    _ => ""
                };

                ConstructClass(output, className);
            }
        }

        /// <summary>
        /// Remove a class on the element.
        /// </summary>
        /// <param name="output">The TagHelperOutput.</param>
        /// <param name="className">The className.</param>
        public static void RemoveClass(TagHelperOutput output, string className)
        {
            if (output != null)
            {
                var existingClass = output.Attributes.FirstOrDefault(f => f.Name == "class");
                var cssClass = string.Empty;
                if (existingClass != null)
                {
                    output.Attributes.Remove(existingClass);
                    cssClass = existingClass.Value.ToString().Replace(className, string.Empty);
                }

                var taClass = new TagHelperAttribute("class", cssClass);
                output.Attributes.Add(taClass);
            }
        }

        /// <summary>
        /// Constructs the identifier.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="id">The identifier.</param>
        public static void ConstructId(TagHelperOutput output, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            var tag = new TagHelperAttribute("id", id);
            output?.Attributes.Add(tag);
        }

        /// <summary>
        /// Constructs the name.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="name">The name.</param>
        public static void ConstructName(TagHelperOutput output, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var tag = new TagHelperAttribute("name", name);
            output?.Attributes.Add(tag);
        }

        /// <summary>
        /// Constructs the href.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="href">The href.</param>
        public static void ConstructHref(TagHelperOutput output, string href)
        {
            if (string.IsNullOrEmpty(href))
            {
                return;
            }

            var tag = new TagHelperAttribute("href", href);
            output?.Attributes.Add(tag);
        }

        /// <summary>
        /// Constructs the value.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="value">The value.</param>
        public static void ConstructValue(TagHelperOutput output, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var tag = new TagHelperAttribute("value", value);
            output?.Attributes.Add(tag);
        }

        /// <summary>
        /// Constructs the type.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="type">The type.</param>
        public static void ConstructType(TagHelperOutput output, string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return;
            }

            var tag = new TagHelperAttribute("type", type);
            output?.Attributes.Add(tag);
        }

        /// <summary>
        /// Constructs the radio input.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        /// <returns>System.String.</returns>
        public static string ConstructRadioInput(string id, string name, string value, bool isChecked)
        {
            var sb = new StringBuilder();
            sb.Append($"<input class='{CssConstants.GovUkRadiosInput}'");
            sb.Append($"id=\"{id}\" name=\"{name}\" type=\"radio\" value=\"{value}\" ");
            if (isChecked)
            {
                sb.Append("checked ");
            }

            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Constructs the radio label.
        /// </summary>
        /// <param name="labelText">The label text.</param>
        /// <param name="forLabel">For label.</param>
        /// <returns>System.String.</returns>
        public static string ConstructRadioLabel(string labelText, string forLabel = null)
        {
            var sb = new StringBuilder();
            sb.Append($"<label class='{CssConstants.GovUkLabel} {CssConstants.GovUkRadiosLabel}'");
            if (forLabel != null)
            {
                sb.Append($"for=\"{forLabel}\"");
            }

            sb.Append($">{labelText}</label>");
            return sb.ToString();
        }

        /// <summary>
        /// Constructs the radio input label.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="labelText">The label text.</param>
        /// <param name="forLabel">For label.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        /// <param name="divider">if set to <c>true</c> [divider].</param>
        /// <param name="dividerText">The divider text.</param>
        /// <param name="hintText">The hint text.</param>
        /// <returns>System.String.</returns>
        public static string ConstructRadioInputLabel(string id, string name, string value, string labelText, string forLabel, bool isChecked = false, bool divider = false, string dividerText = null, string hintText = null, string inputText = null, string inputType = null, string fileUploadHint = null)
        {
            var sb = new StringBuilder();
            if (divider)
            {
                sb.Append($"<div class=\"{CssConstants.GovUkRadioDivider}\">");
                sb.Append($"{dividerText}");
                sb.Append("</div>");
            }
            else
            {
                sb.Append($"<div class=\"{CssConstants.GovUkRadiosItem}\">");
                sb.Append(ConstructRadioInput(id, name, value, isChecked));
                sb.Append(ConstructRadioLabel(labelText, forLabel));
                if (hintText != null)
                {
                    sb.Append($"<span class=\"{CssConstants.GovUkHint} {CssConstants.GovUkRadiosHint}\">");
                    sb.Append($"{hintText}");
                    sb.Append("</span>");
                }
                if(inputText != null)
                {
                    sb.Append(
                            $"<div class=\"govuk-radios__conditional govuk-radios__conditional--hidden\" id=\"{id}-conditional\">\r\n " +
                                "<div class=\"govuk-form-group\">\r\n          " +
                                    $"<label class=\"govuk-label\" for=\"{inputText}\">\r\n            " +
                                        $"{inputText}\r\n          " +
                                    "</label>\r\n          " +
                                    $"<textarea class=\"govuk-textarea\" rows=\"5\" id=\"{id}-input\" name=\"{inputText}\" type=\"{(hintText is null ? "text" : hintText)}\" spellcheck=\"false\" autocomplete=\"email\"></textarea> \r\n        " +
                                "</div>" +
                                $"<div class=\"govuk-hint\">{fileUploadHint}</div>");
                    
                    if(fileUploadHint != null)
                        sb.Append("<div class=\"govuk-form-group\">\r\n  <label class=\"govuk-label\" for=\"file-upload-1\">\r\n    Upload a file\r\n  </label>\r\n  <input class=\"govuk-file-upload\" id=\"file-upload-1\" name=\"file-upload-1\" type=\"file\">\r\n</div>");

                    sb.Append("</div>");
                }

                sb.Append("</div>");
            }

            return sb.ToString();
        }

        public static RadioBuilder CreateRadio()
        {
            return new RadioBuilder();
        }

        /// <summary>
        /// Constructs the anchor.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="text">The text.</param>
        /// <returns>System.String.</returns>
        public static string ConstructAnchor(string href, string text)
        {
            var sb = new StringBuilder();
            sb.Append($"<a href='{href}' class='{CssConstants.GovUkLink}'>");
            sb.Append($"{text}");
            sb.Append("</a>");
            return sb.ToString();
        }

        /// <summary>
        /// Constructs the ul lists.
        /// </summary>
        /// <param name="lists">The lists.</param>
        /// <returns>System.String.</returns>
        public static string ConstructUlLists(string[] lists)
        {
            var sb = new StringBuilder();
            if (lists == null)
            {
                return null;
            }

            foreach (var list in lists)
            {
                sb.Append($"<li>{list}</li>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Constructs the button start.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>System.String.</returns>
        public static string ConstructButtonStart(string text)
        {
            var sb = new StringBuilder();
            sb.Append($"{text}");
            sb.Append("<svg class=\"govuk-button__start-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"17.5\" height=\"19\" viewBox=\"0 0 33 40\" aria-hidden=\"true\" focusable=\"false\">");
            sb.Append("<path fill=\"currentColor\" d=\"M0 0h13l20 20-20 20H0l20-20z\"></path>");
            sb.Append("</svg>");
            return sb.ToString();
        }

        /// <summary>
        /// Constructs the summary list group.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="href">The href.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="visuallyHidden">The visually hidden.</param>
        /// <returns>System.String.</returns>
        public static string ConstructSummaryListGroup(string key, string value, string href, string linkText, string visuallyHidden)
        {
            var sb = new StringBuilder();

            // sb.Append($"<div id=\"{id}\" class=\"{Constants.GovUkSummaryListRowCss}\">");
            sb.Append($"<dt class=\"{CssConstants.GovUkSummaryListKey}\">");
            sb.Append($"{key}");
            sb.Append("</dt>");

            sb.Append($"<dd class=\"{CssConstants.GovUkSummaryListValue}\">");
            sb.Append($"{value}");
            sb.Append("</dd>");

            sb.Append($"<dd class=\"{CssConstants.GovUkSummaryListActions}\">");
            sb.Append($"<a class=\"{CssConstants.GovUkLink}\" href=\"{href}\">");
            sb.Append($"{linkText}");
            if (visuallyHidden != null)
            {
                sb.Append($"<span class=\"{CssConstants.GovVisuallyHidden}\">{visuallyHidden}</span>");
            }

            sb.Append("</a>");
            sb.Append("</dd>");

            // sb.Append("</div>");
            return sb.ToString();
        }

        /// <summary>
        /// Construct a generic Attribute for the element.
        /// </summary>
        /// <param name="output">The TagHelperOutput.</param>
        /// <param name="attributeName">The attribute name.</param>
        /// <param name="attributeValue">The attribute value.</param>
        public static void ConstructGenericAttribute(TagHelperOutput output, string attributeName, string attributeValue)
        {
            if (attributeName.IsNullOrEmpty() || attributeValue.IsNullOrEmpty())
            {
                return;
            }

            output?.Attributes.SetAttribute(new TagHelperAttribute(attributeName, attributeValue));
        }

        /// <summary>
        /// Constructs the generic.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="text">The text.</param>
        public static void ConstructGeneric(TagHelperOutput output, string attribute, string text)
        {
            if (string.IsNullOrEmpty(attribute) || string.IsNullOrEmpty(text))
            {
                return;
            }

            output?.Attributes.Add(new TagHelperAttribute(attribute, text));
        }

        /// <summary>
        /// Construct a generic Attribute for the element.
        /// </summary>
        /// <param name="output">The TagHelperOutput.</param>
        public static void RemoveGdsFromTagName(TagHelperOutput output)
        {
            if (output != null && output.TagName.Contains("gds-"))
            {
                output.TagName = output.TagName.Replace("gds-", string.Empty);
            }
        }

        /// <summary>Construct a generic Attribute for the element.</summary>
        /// <param name="text">The text.</param>
        /// <returns>System.String.</returns>
        public static string RemoveSymbolsHtmFieldName(string text)
        {
            if (text.Contains("["))
            {
                text = text.Replace("[", "_");
            }

            if (text.Contains("]"))
            {
                text = text.Replace("]", "_");
            }

            if (text.Contains("."))
            {
                text = text.Replace(".", "_");
            }

            return text;
        }
    }
}
