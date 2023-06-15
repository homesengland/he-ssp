using He.AspNetCore.Mvc.Gds.Components.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace He.AspNetCore.Mvc.Gds.Components.TagConstructs
{
    public class RadioBuilder
    {
        private bool _isDivider;
        private string _dividerText;
        private string _id;
        private string _name;
        private string _value;
        private string _hintText;
        private string _label;
        private string _forLabel;
        private string _inputLabel;
        private string _inputName;
        private string _innerInputValue;
        private string _fileUploadLabel;
        private bool _checked;
        private string _conditionalError;
        private bool _isConditionalInvalid;
        private string _radioHint;

        public RadioBuilder()
        {
            _checked = false;
        }

        public RadioBuilder AsDivider(string dividerText)
        {
            _dividerText = dividerText;
            _isDivider = true;

            return this;
        }

        public RadioBuilder AsRadio(string id, string name, string value)
        {
            _id = id;
            _name = name;
            _value = value; 

            return this;
        }

        public RadioBuilder ThatIsChecked()
        {
            _checked = true;

            return this;
        }

        public RadioBuilder WithHint(string hintText)
        {
            _hintText = hintText;
            
            return this;
        }

        public RadioBuilder WithLabel(string label, string forLabel)
        {
            _label = label;
            _forLabel = forLabel;

            return this;
        }

        public RadioBuilder WithConditionalInput(string inputLabel, string inputName, string innerInputValue = null)
        {
            _inputLabel = inputLabel;
            _inputName = inputName;
            _innerInputValue = innerInputValue;

            return this;
        }

        public RadioBuilder WithFileUpload(string fileUploadLabel)
        {
            _fileUploadLabel = fileUploadLabel;

            return this;
        }
        public RadioBuilder WithRadioHint(string radioHint)
        {
            _radioHint = radioHint;

            return this;
        }

        public RadioBuilder WithConditionalErrorMessage(string conditionalInputError)
        {
            _conditionalError = conditionalInputError;
            _isConditionalInvalid = true;

            return this;
        }

        public string Build()
        {
            var sb = new StringBuilder();
            if (_isDivider)
            {
                sb.Append($"<div class=\"{CssConstants.GovUkRadioDivider}\">");
                sb.Append($"{_dividerText}");
                sb.Append("</div>");
            }
            else
            {
                sb.Append($"<div class=\"{CssConstants.GovUkRadiosItem}\">");
                sb.Append(TagConstruct.ConstructRadioInput(_id, _name, _value, _checked));
                sb.Append(TagConstruct.ConstructRadioLabel(_label, _forLabel));

                if (_hintText != null)
                {
                    sb.Append($"<span class=\"{CssConstants.GovUkHint} {CssConstants.GovUkRadiosHint}\">");
                    sb.Append($"{_hintText}");
                    sb.Append("</span>");
                }
                if (_inputLabel != null)
                {
                    if (_isConditionalInvalid)
                    {
                        sb.Append($"<div class=\"govuk-radios__conditional govuk-radios__conditional--hidden \" id=\"{_id}-conditional\">" +
                                    "<div class=\"govuk-form-group govuk-form-group--error\">" +
                                        $"<label class=\"govuk-label\" for=\"{_inputName ?? "conditional-input"}\">" +
                                            $"{_inputLabel}" +
                                            $"<p id=\"contact-by-email-error\" class=\"govuk-error-message\">  " +
                                                $"<span class=\"govuk-visually-hidden\">Error:</span> {_conditionalError}" +
                                            $"</p>" +
                                        "</label>");

                        if(!string.IsNullOrEmpty(_radioHint))
                        {
                            sb.Append($"<div id=\"sign-in-hint\" class=\"govuk-hint\">{_radioHint}</div>");
                        }

                        sb.Append( 
                                $"<textarea class=\"govuk-textarea govuk-textarea--error\" rows=\"5\" id=\"{_id}-input\" name=\"{_inputName ?? "conditional-input"}\" type=\"{(_hintText is null ? "text" : _hintText)}\" spellcheck=\"false\" autocomplete=\"email\">{_innerInputValue}</textarea>" +
                            "</div>"
                        );
                    }
                    else
                    {
                        sb.Append($"<div class=\"govuk-radios__conditional govuk-radios__conditional--hidden\" id=\"{_id}-conditional\">\r\n " +
                                    "<div class=\"govuk-form-group\">" +
                                        $"<label class=\"govuk-label\" for=\"{_inputName ?? "conditional-input"}\">  " +
                                            $"{_inputLabel}" +
                                        "</label>");

                        if (!string.IsNullOrEmpty(_radioHint))
                        {
                            sb.Append($"<div id=\"sign-in-hint\" class=\"govuk-hint\">{_radioHint}</div>");
                        }

                        sb.Append(
                               $"<textarea class=\"govuk-textarea\" rows=\"5\" id=\"{_id}-input\" name=\"{_inputName ?? "conditional-input"}\" type=\"{(_hintText is null ? "text" : _hintText)}\" spellcheck=\"false\" autocomplete=\"email\">{_innerInputValue}</textarea>"  +
                            "</div>"
                        );
                    }

                    if (_fileUploadLabel != null)
                    {
                        sb.Append($"<div class=\"govuk-hint\">{_fileUploadLabel}</div>");
                        sb.Append("<div class=\"govuk-form-group\">\r\n  <label class=\"govuk-label\" for=\"file-upload-1\">\r\n    Upload a file\r\n  </label>\r\n  <input class=\"govuk-file-upload\" id=\"file-upload-1\" name=\"file-upload-1\" type=\"file\">\r\n</div>");
                    }

                    sb.Append("</div>");
                }

                sb.Append("</div>");
            }

            return sb.ToString();
        }

        public string ConditionalInput(string innerContent)
        {
            return $"<div class=\"govuk-radios__conditional govuk-radios__conditional--hidden \" id=\"{_id}-conditional\">" +
                    innerContent +
                   $"</div>";
        }

        public string ErrorFormGroup(string innerContent)
        {
            return "<div class=\"govuk-form-group govuk-form-group--error\">" +
                "" +
                "";
        }

        public string InputLabel(string innerContent)
        {
            return "";
        }

        public string InputError(string innerContent)
        {
            return "";
        }

        public string TextArea()
        {
            return "";
        }
    }
}
