using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Components.SelectList;

namespace HE.Investments.Common.WWW.Tests.Components.SelectListTests;

[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "ViewComponents tests are failing on CI from time to time.")]
public class SelectListTests : ViewComponentTestBase<SelectListTests>
{
    private const string ViewPath = "/Components/SelectListTests/SelectListTests.cshtml";

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayViewWithoutPagination_WhenThereIsOnePage()
    {
        // given
        var model = CreateTestModel(totalItems: 1);

        // when
        var document = await Render(ViewPath, model);

        // then
        foreach (var item in model.Items.Items)
        {
            document.HasSelectListItem(item.Text, item.Description);
        }

        document
            .HasLinkButton(model.AddActionText, model.AddActionUrl)
            .HasPagination(false);
    }

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayViewWithPagination_WhenThereAreMoreThanOnePage()
    {
        // given
        var model = CreateTestModel();

        // when
        var document = await Render(ViewPath, model);

        // then
        foreach (var item in model.Items.Items)
        {
            document.HasSelectListItem(item.Text, item.Description);
        }

        document
            .HasLinkButton(model.AddActionText, model.AddActionUrl)
            .HasPagination();
    }

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayViewWithoutAddButton_WhenAddActionUrlNotProvided()
    {
        // given
        var model = CreateTestModel(null);

        // when
        var document = await Render(ViewPath, model);

        // then
        foreach (var item in model.Items.Items)
        {
            document.HasSelectListItem(item.Text, item.Description);
        }

        document
            .HasNoButton()
            .HasPagination();
    }

    private static SelectListTestModel CreateTestModel(string? addUrl = "AddActionUrl", int totalItems = 2)
    {
        var items = new List<SelectListItemViewModel> { new("Url", "One", "Desc one"), new("Url", "Two", null), new("Url", "Three", "Desc three"), };

        var pagination = new PaginationResult<SelectListItemViewModel>(items, 1, 1, totalItems);
        return new SelectListTestModel(pagination, "PagingUrl", addUrl, "ActionText");
    }
}

public record SelectListTestModel(PaginationResult<SelectListItemViewModel> Items, string PagingNavigationUrl, string? AddActionUrl, string AddActionText);
