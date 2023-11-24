using AngleSharp.Dom;

namespace HE.Investments.Loans.IntegrationTests.IntegrationFramework.Assertions;

public static class ElementExtensions
{
    public static ElementFluentAssertions Should(this IElement? element)
    {
        return new ElementFluentAssertions(element);
    }
}
