using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentInputExtensions
{
    public static IHtmlDocument HasTextAreaInput(
        this IHtmlDocument htmlDocument,
        string fieldName,
        string? label = null,
        string? value = null,
        bool exist = true)
    {
        var inputs = GetAndValidateSingleInput<IHtmlTextAreaElement>(htmlDocument, fieldName, label, exist);

        if (!exist)
        {
            return htmlDocument;
        }

        if (!string.IsNullOrEmpty(value))
        {
            inputs!.InnerHtml.Should().Contain(value);
        }

        return htmlDocument;
    }

    public static IHtmlDocument HasInput(this IHtmlDocument htmlDocument, string fieldName, string? label = null, string? value = null, bool exist = true)
    {
        var inputs = GetAndValidateSingleInput<IHtmlInputElement>(htmlDocument, fieldName, label, exist);

        if (!exist)
        {
            return htmlDocument;
        }

        if (!string.IsNullOrEmpty(value))
        {
            inputs!.Value.Should().Contain(value);
        }

        return htmlDocument;
    }

    public static IHtmlDocument HasDateInput(
        this IHtmlDocument htmlDocument,
        string fieldName,
        string title,
        string? day = null,
        string? month = null,
        string? year = null,
        bool exist = true)
    {
        var dayInput = GetAndValidateSingleInput<IHtmlInputElement>(htmlDocument, $"{fieldName}.Day", exist: exist);
        var monthInput = GetAndValidateSingleInput<IHtmlInputElement>(htmlDocument, $"{fieldName}.Month", exist: exist);
        var yearInput = GetAndValidateSingleInput<IHtmlInputElement>(htmlDocument, $"{fieldName}.Year", exist: exist);

        if (!exist)
        {
            return htmlDocument;
        }

        var header = htmlDocument.GetElements(".govuk-fieldset__legend .govuk-heading-m", title).FirstOrDefault();
        header.Should().NotBeNull($"Cannot find title for {fieldName}");

        if (!string.IsNullOrEmpty(day))
        {
            dayInput!.Value.Should().Contain(day);
        }

        if (!string.IsNullOrEmpty(month))
        {
            monthInput!.Value.Should().Contain(month);
        }

        if (!string.IsNullOrEmpty(year))
        {
            yearInput!.Value.Should().Contain(year);
        }

        return htmlDocument;
    }

    public static IHtmlDocument HasGdsRadioInputWithValues(this IHtmlDocument htmlDocument, string fieldName, params string[] values)
    {
        var gdsRadioInputs = htmlDocument.GetElementsByName(fieldName);
        gdsRadioInputs.Should().NotBeEmpty($"GDS Radio input for field {fieldName} should exist");
        foreach (var gdsRadioInput in gdsRadioInputs)
        {
            gdsRadioInput.ClassName.Should().Contain("govuk-radios__input");
            var radioInput = (IHtmlInputElement)gdsRadioInput;
            values.Should().Contain(radioInput.Value, $"Radio input value should have one of the expected values {string.Join(',', values)}");
        }

        return htmlDocument;
    }

    public static IHtmlDocument HasRadio(this IHtmlDocument htmlDocument, string fieldName, IList<string>? options = null, string? value = null, bool exist = true)
    {
        var inputs = GetAndValidateInputs<IElement>(htmlDocument, fieldName, exist: exist);

        if (!exist)
        {
            return htmlDocument;
        }

        if (options != null)
        {
            inputs.Count.Should().Be(options.Count, $"{options.Count} inputs with name {fieldName} should exist");
        }

        if (!string.IsNullOrWhiteSpace(value))
        {
            var selected = inputs.FirstOrDefault(i => i.IsChecked());

            selected.Should().NotBeNull($"Radio input with name {fieldName} should be checked.");
            selected!.Attributes.FirstOrDefault(a => a.Name == "value")!.Value.Should()
                .Be(value, $"Radio input with name {fieldName} should have value {value}.");
        }

        return htmlDocument;
    }

    public static IHtmlDocument HasBoldText(this IHtmlDocument htmlDocument, string text)
    {
        var boldText = htmlDocument.GetElements("b", text);
        boldText.Count.Should().Be(1, $"Only one bold text with innerText {text} should exist");

        return htmlDocument;
    }

    public static IHtmlDocument HasHeader2(this IHtmlDocument htmlDocument, string text, bool exists = true)
    {
        var headerText = htmlDocument.GetElements("h2", text);

        if (exists)
        {
            headerText.Should().ContainSingle($"Only one bold text with innerText {text} should exist");
        }
        else
        {
            headerText.Should().BeEmpty($"No bold text with innerText {text} should exist");
        }

        return htmlDocument;
    }

#pragma warning disable S4144 // Methods should not have identical implementations
    public static IHtmlDocument HasCheckboxes(this IHtmlDocument htmlDocument, string fieldName, IList<string> options)
#pragma warning restore S4144 // Methods should not have identical implementations
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);
        inputs.Length.Should().Be(options.Count, $"{options.Count} inputs with name {fieldName} should exist");

        return htmlDocument;
    }

    public static IHtmlDocument HasCheckedCheckboxes(this IHtmlDocument htmlDocument, string fieldName, IList<string> checkedValues)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);

        foreach (var checkedValue in checkedValues)
        {
            var element = inputs?.FirstOrDefault(x => x.OuterHtml.Contains(checkedValue));
            var isChecked = element?.IsChecked();
            isChecked.Should().Be(true, $"{checkedValue} value should be checked");
        }

        return htmlDocument;
    }

    private static T? GetAndValidateSingleInput<T>(IHtmlDocument htmlDocument, string fieldName, string? label = null, bool exist = true)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);

        if (exist)
        {
            inputs.Length.Should().Be(1, $"Only one element input with name {fieldName} should exist");

            if (!string.IsNullOrEmpty(label))
            {
                var labels = htmlDocument.GetLastChildByTagAndText("label", label);
                labels.Count.Should().Be(1, $"Only one element input with label with innerText {label} should exist");
            }

            return inputs.Cast<T>().First();
        }

        inputs.Length.Should().Be(0, $"Element input with name {fieldName} should not exist");

        return default;
    }

    private static IList<T> GetAndValidateInputs<T>(IHtmlDocument htmlDocument, string fieldName, string? label = null, bool exist = true)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);

        if (exist)
        {
            inputs.Length.Should().BePositive($"At least one element input with name {fieldName} should exist");

            if (!string.IsNullOrEmpty(label))
            {
                var labels = htmlDocument.GetLastChildByTagAndText("label", label);
                labels.Count.Should().Be(1, $"Only one element input with label with innerText {label} should exist");
            }
        }
        else
        {
            inputs.Length.Should().Be(0, $"Element input with name {fieldName} should not exist");
        }

        return inputs.Cast<T>().ToList();
    }
}
