using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Contract;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investments.Common.WWW.Tests.Components.DateInputTests;

[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "ViewComponents tests are failing on CI from time to time.")]
public class DateInputTests : ViewComponentTestBase<DateInputTests>
{
    private const string Error = "test error";
    private const string ViewPath = "/Components/DateInputTests/DateInputTests.cshtml";
    private readonly DateInputTestsViewModel _model = new(new DateDetails("33", "5", "2024"));

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayViewForMissingModel()
    {
        // given & when
        var document = await Render<DateInputTestsViewModel>(ViewPath);

        // then
        Assert(document);
    }

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(ViewPath, _model);

        // then
        Assert(document, _model);
    }

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayViewWithErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError($"{nameof(DateInputTestsViewModel.StartDate)}", Error);

        // when
        var document = await Render(ViewPath, _model, modelStateDictionary: modelState);

        // then
        Assert(document, _model);
        document.HasErrorMessage($"{nameof(DateInputTestsViewModel.StartDate)}", Error);
    }

    private static void Assert(IHtmlDocument document, DateInputTestsViewModel? model = null)
    {
        document
            .HasElementWithText("h1", "Enter start date")
            .HasElementWithText("div", "This can be a date in the past. For future dates, include a reasonable level of risk adjustment in your forecasts.")
            .HasElementWithText("div", "Enter date, for example: 11 05 2024")
            .HasInput($"{nameof(DateInputTestsViewModel.StartDate)}.Day", "Day", model?.StartDate.Day)
            .HasInput($"{nameof(DateInputTestsViewModel.StartDate)}.Month", "Month", model?.StartDate.Month)
            .HasInput($"{nameof(DateInputTestsViewModel.StartDate)}.Year", "Year", model?.StartDate.Year);
    }
}
