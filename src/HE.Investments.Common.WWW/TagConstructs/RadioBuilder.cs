using System.Globalization;
using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;

namespace HE.Investments.Common.WWW.TagConstructs;

public class RadioBuilder
{
    private bool _isDivider;
    private string? _dividerText;
    private string? _id;
    private string? _name;
    private string? _value;
    private string? _hintText;
    private string? _label;
    private string? _forLabel;
    private string? _innerInputId;
    private string? _inputLabel;
    private string? _inputName;
    private string? _innerInputValue;
    private bool _checked;
    private string? _conditionalError;
    private bool _isConditionalInvalid;

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

    public RadioBuilder WithConditionalInput(string inputId, string inputLabel, string inputName, string? innerInputValue = null)
    {
        _innerInputId = inputId;
        _inputLabel = inputLabel;
        _inputName = inputName;
        _innerInputValue = innerInputValue;

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
            sb.Append(CultureInfo.InvariantCulture, $"{_dividerText}");
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
                sb.Append(CultureInfo.InvariantCulture, $"{_hintText}");
                sb.Append("</span>");
            }

            if (_inputLabel != null)
            {
                sb.Append("</div>");
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

                    sb.Append(
                        $"<textarea class=\"govuk-textarea govuk-textarea--error\" rows=\"5\" id=\"{_innerInputId}\" name=\"{_inputName ?? "conditional-input"}\" type=\"{_hintText ?? "text"}\" spellcheck=\"false\" autocomplete=\"email\">{_innerInputValue}</textarea>" +
                        "</div>");
                }
                else
                {
                    sb.Append($"<div class=\"govuk-radios__conditional govuk-radios__conditional--hidden\" id=\"{_id}-conditional\">\r\n " +
                                "<div class=\"govuk-form-group\">" +
                                    $"<label class=\"govuk-label\" for=\"{_inputName ?? "conditional-input"}\">  " +
                                        $"{_inputLabel}" +
                                    "</label>");

                    sb.Append(
                        $"<textarea class=\"govuk-textarea\" rows=\"5\" id=\"{_innerInputId}\" name=\"{_inputName ?? "conditional-input"}\" type=\"{_hintText ?? "text"}\" spellcheck=\"false\" autocomplete=\"email\">{_innerInputValue}</textarea>" +
                        "</div>");
                }
            }

            sb.Append("</div>");
        }

        return sb.ToString();
    }
}
