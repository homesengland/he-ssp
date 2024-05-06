using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlElementFilters
{
    public static List<IElement> WithText(IList<IElement> elements, string? text = null)
    {
        if (!string.IsNullOrEmpty(text))
        {
            elements = elements
                .Where(i => i.TextContent.Contains(text))
                .ToList();

            elements
                .Any()
                .Should()
                .BeTrue($"There is no Element with text: '{text}'");
        }

        return [.. elements];
    }

    public static List<IElement> WithClass(IList<IElement> elements, string? className = null)
    {
        if (!string.IsNullOrEmpty(className))
        {
            elements = elements
                .Where(i => i.ClassList.Contains(className))
                .ToList();

            elements
                .Any()
                .Should()
                .BeTrue($"There is no Element with class: '{className}'");
        }

        return [.. elements];
    }

    public static List<IElement> WithAttribute(IList<IElement> elements, string? attributeName = null, string? value = null)
    {
        if (!string.IsNullOrEmpty(value))
        {
            elements = elements
                .Where(i => i.Attributes.Any(a => a.Name == attributeName && a.Value.Contains(value)))
                .ToList();

            elements
                .Any()
                .Should()
                .BeTrue($"There is no Element with attribute : '{attributeName}' value: '{value}'");
        }

        return [.. elements];
    }
}
