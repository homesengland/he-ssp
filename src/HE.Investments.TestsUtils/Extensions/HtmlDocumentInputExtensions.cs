using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentInputExtensions
{
    public static IHtmlDocument HasTextAreaInput(this IHtmlDocument htmlDocument, string fieldName, string? label = null, string? value = null)
    {
        var inputs = GetAndValidateInput<IHtmlTextAreaElement>(htmlDocument, fieldName, label);

        if (!string.IsNullOrEmpty(value))
        {
            inputs.First().InnerHtml.Should().Contain(value);
        }

        return htmlDocument;
    }

    public static IHtmlDocument HasInput(this IHtmlDocument htmlDocument, string fieldName, string? label = null, string? value = null)
    {
        var inputs = GetAndValidateInput<IHtmlInputElement>(htmlDocument, fieldName, label);

        if (!string.IsNullOrEmpty(value))
        {
            inputs.First().Value.Should().Contain(value);
        }

        return htmlDocument;
    }

    public static IHtmlDocument HasGdsInput(this IHtmlDocument htmlDocument, string fieldName)
    {
        var gdsInput = htmlDocument.GetElementsByName(fieldName).SingleOrDefault();
        gdsInput.Should().NotBeNull($"GDS input for field {fieldName} should exist");
        gdsInput!.ClassName.Should().Contain("govuk-input");
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

    public static IHtmlDocument HasRadio(this IHtmlDocument htmlDocument, string fieldName, IList<string> options)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);
        inputs.Length.Should().Be(options.Count, $"{options.Count} inputs with name {fieldName} should exist");

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


    private static IEnumerable<T> GetAndValidateInput<T>(IHtmlDocument htmlDocument, string fieldName, string? label)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);
        inputs.Length.Should().Be(1, $"Only one element input with name {fieldName} should exist");

        if (!string.IsNullOrEmpty(label))
        {
            var labels = htmlDocument.GetLastChildByTagAndText("label", label);
            labels.Count.Should().Be(1, $"Only one element input with label with innerText {label} should exist");
        }

        return inputs.Cast<T>();
    }
}
