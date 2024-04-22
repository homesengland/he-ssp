using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.Extensions;

public static class ApplicationListFluentExtensions
{
    public static IHtmlDocument HasApplicationInStatus(this IHtmlDocument htmlDocument, string applicationId, ApplicationStatus applicationStatus)
    {
        var applicationRow = htmlDocument.GetElementByTestId(applicationId);
        var tagElement = applicationRow.GetElementsByClassName("govuk-tag").FirstOrDefault();
        tagElement.Should().NotBeNull();

        tagElement!.Text().Trim().Should().Be(applicationStatus.GetDescription());

        return htmlDocument;
    }
}
