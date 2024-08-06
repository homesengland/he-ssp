using FluentAssertions;
using HE.Investment.AHP.WWW.Models.AllocationClaim.Factories;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.Common.WWW.Components.SectionSummary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Moq;

namespace HE.Investment.AHP.WWW.Tests.Models.AllocationClaim.Factories;

public class AllocationClaimCheckAnswersViewModelFactoryTests
{
    [Fact]
    public void ShouldReturnAllSummaryItems_WhenMilestoneTypeIsAcquisition()
    {
        // given
        var allocationId = AllocationId.From(GuidTestData.GuidOne);
        var phaseId = PhaseId.From(GuidTestData.GuidTwo);
        var claim = new MilestoneClaim(
            MilestoneType.Acquisition,
            MilestoneStatus.Due,
            1000,
            10,
            new DateDetails("10", "10", "2024"),
            new DateDetails("12", "12", "2024"),
            null,
            true,
            true,
            true,
            true);
        var urlHelper = MockUrlHelper();

        // when
        var result = new AllocationClaimCheckAnswersViewModelFactory().CreateSummary(allocationId, phaseId, claim, urlHelper, true);

        // then
        result.Items.Should().HaveCount(4);
        result.Title.Should().Be("Claim summary");
        result.Items
            .Should()
            .BeEquivalentTo(
                new[]
                {
                    new SectionSummaryItemModel("Cost incurred", ["Yes"], "AllocationClaim/CostsIncurred", IsEditable: true, IsVisible: true),
                    new SectionSummaryItemModel("Amount of grant apportioned to acquisition milestone", ["\u00a31,000"], IsEditable: true, IsVisible: true),
                    new SectionSummaryItemModel("Acquisition achievement date", ["12 December 2024"], "AllocationClaim/AchievementDate", IsEditable: true, IsVisible: true),
                    new SectionSummaryItemModel("Acquisition is confirmed", ["Yes"], "AllocationClaim/Confirmation", IsEditable: true, IsVisible: false),
                });
    }

    [Fact]
    public void ShouldReturnAllSummaryItemsExceptCostIncurred_WhenMilestoneTypeIsCompletion()
    {
        // given
        var allocationId = AllocationId.From(GuidTestData.GuidOne);
        var phaseId = PhaseId.From(GuidTestData.GuidTwo);
        var claim = new MilestoneClaim(
            MilestoneType.Completion,
            MilestoneStatus.Due,
            9999,
            10,
            new DateDetails("10", "10", "2024"),
            new DateDetails("25", "1", "2024"),
            null,
            true,
            true,
            true,
            true);
        var urlHelper = MockUrlHelper();

        // when
        var result = new AllocationClaimCheckAnswersViewModelFactory().CreateSummary(allocationId, phaseId, claim, urlHelper, true);

        // then
        result.Items.Should().HaveCount(3);
        result.Title.Should().Be("Claim summary");
        result.Items
            .Should()
            .BeEquivalentTo(
                new[]
                {
                    new SectionSummaryItemModel("Amount of grant apportioned to practical completion milestone", ["\u00a39,999"], IsEditable: true, IsVisible: true),
                    new SectionSummaryItemModel("Practical completion achievement date", ["25 January 2024"], "AllocationClaim/AchievementDate", IsEditable: true, IsVisible: true),
                    new SectionSummaryItemModel("Practical completion is confirmed", ["Yes"], "AllocationClaim/Confirmation", IsEditable: true, IsVisible: false),
                });
    }

    private static IUrlHelper MockUrlHelper()
    {
        var urlHelper = new Mock<IUrlHelper>();
        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
        };

        urlHelper.Setup(x => x.ActionContext).Returns(actionContext);

        urlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
            .Returns<UrlActionContext>(context =>
            {
                var queryParameters = context.Values!.GetType()
                    .GetProperties()
                    .Where(x => x.GetValue(context.Values, null) != null)
                    .ToDictionary(
                        x => x.Name,
                        x => x.GetValue(context.Values, null)!.ToString());

                return QueryHelpers.AddQueryString($"{context.Controller}/{context.Action}", queryParameters);
            });

        return urlHelper.Object;
    }
}
