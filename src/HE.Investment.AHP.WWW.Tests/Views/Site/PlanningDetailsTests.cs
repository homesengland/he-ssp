using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class PlanningDetailsTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Site/PlanningDetails.cshtml";

    [Theory]
    [InlineData(true, false, false, false, false, false, false, false, false)]
    [InlineData(false, true, false, false, false, false, false, false, false)]
    [InlineData(false, false, true, false, false, false, false, false, false)]
    [InlineData(false, false, false, true, false, false, false, false, false)]
    [InlineData(false, false, false, false, true, false, false, false, false)]
    [InlineData(false, false, false, false, false, true, false, false, false)]
    [InlineData(false, false, false, false, false, false, true, false, false)]
    [InlineData(false, false, false, false, false, false, false, false, true)]
    public async Task ShouldDisplayView(
        bool referenceNumberExist,
        bool detailedPlanningApprovalDateExist,
        bool applicationForDetailedPlanningSubmittedDateExist,
        bool outlinePlanningApprovalDateExist,
        bool planningSubmissionDateExist,
        bool expectedPlanningApprovalDateExist,
        bool isGrantFundingForAllHomesExist,
        bool requiredFurtherStepsExist,
        bool isLandRegistryTitleNumberRegisteredExist)
    {
        var model = CreateTestModel(
            referenceNumberExist,
            detailedPlanningApprovalDateExist,
            applicationForDetailedPlanningSubmittedDateExist,
            outlinePlanningApprovalDateExist,
            planningSubmissionDateExist,
            expectedPlanningApprovalDateExist,
            isGrantFundingForAllHomesExist,
            requiredFurtherStepsExist,
            isLandRegistryTitleNumberRegisteredExist);
        var viewBag = new Dictionary<string, object> { { "SiteName", " some site name" } };

        // given & when
        var document = await Render(_viewPath, model, viewBag);

        // then
        document
            .HasPageHeader(viewBag["SiteName"].ToString())
            .HasInput(
                nameof(SiteModel.PlanningDetails.ReferenceNumber),
                "Enter your planning reference number",
                model.ReferenceNumber,
                exist: referenceNumberExist)
            .HasDateInput(
                nameof(SitePlanningDetails.DetailedPlanningApprovalDate),
                "Enter when detailed planning approval was granted",
                day: model.DetailedPlanningApprovalDate?.Day,
                month: model.DetailedPlanningApprovalDate?.Month,
                year: model.DetailedPlanningApprovalDate?.Year,
                exist: detailedPlanningApprovalDateExist)
            .HasDateInput(
                nameof(SitePlanningDetails.ApplicationForDetailedPlanningSubmittedDate),
                "Enter when your application for detailed planning permission was submitted",
                day: model.ApplicationForDetailedPlanningSubmittedDate?.Day,
                month: model.ApplicationForDetailedPlanningSubmittedDate?.Month,
                year: model.ApplicationForDetailedPlanningSubmittedDate?.Year,
                exist: applicationForDetailedPlanningSubmittedDateExist)
            .HasDateInput(
                nameof(SitePlanningDetails.OutlinePlanningApprovalDate),
                "Enter when outline planning approval was granted",
                day: model.OutlinePlanningApprovalDate?.Day,
                month: model.OutlinePlanningApprovalDate?.Month,
                year: model.OutlinePlanningApprovalDate?.Year,
                exist: outlinePlanningApprovalDateExist)
            .HasDateInput(
                nameof(SitePlanningDetails.PlanningSubmissionDate),
                "Enter when your application for outline planning permission was submitted",
                day: model.PlanningSubmissionDate?.Day,
                month: model.PlanningSubmissionDate?.Month,
                year: model.PlanningSubmissionDate?.Year,
                exist: planningSubmissionDateExist)
            .HasDateInput(
                nameof(SitePlanningDetails.ExpectedPlanningApprovalDate),
                "Enter when you expect to get detailed planning approval",
                day: model.ExpectedPlanningApprovalDate?.Day,
                month: model.ExpectedPlanningApprovalDate?.Month,
                year: model.ExpectedPlanningApprovalDate?.Year,
                exist: expectedPlanningApprovalDateExist)
            .HasRadio(
                nameof(SitePlanningDetails.IsGrantFundingForAllHomesCoveredByApplication),
                new List<string> { "Yes", "No" },
                value: "True",
                exist: isGrantFundingForAllHomesExist)
            .HasTextAreaInput(
                nameof(SitePlanningDetails.RequiredFurtherSteps),
                "Tell us the further steps required before start on site can occur",
                model.RequiredFurtherSteps,
                requiredFurtherStepsExist)
            .HasRadio(
                nameof(SitePlanningDetails.IsLandRegistryTitleNumberRegistered),
                new List<string> { "Yes", "No" },
                value: "False",
                exist: isLandRegistryTitleNumberRegisteredExist)
            .HasSaveAndContinueButton()
            .HasBackLink(false);
    }

    private static SitePlanningDetails CreateTestModel(
        bool referenceNumberExist,
        bool detailedPlanningApprovalDateExist,
        bool applicationForDetailedPlanningSubmittedDateExist,
        bool outlinePlanningApprovalDateExist,
        bool planningSubmissionDateExist,
        bool expectedPlanningApprovalDateExist,
        bool isGrantFundingForAllHomesExist,
        bool requiredFurtherStepsExist,
        bool isLandRegistryTitleNumberRegisteredExist)
    {
        return new SitePlanningDetails(
            SitePlanningStatus.Undefined,
            "ref123",
            referenceNumberExist,
            new DateDetails("1", "2", "2023"),
            detailedPlanningApprovalDateExist,
            "steps required",
            requiredFurtherStepsExist,
            new DateDetails("1", "2", "2023"),
            applicationForDetailedPlanningSubmittedDateExist,
            new DateDetails("1", "2", "2023"),
            expectedPlanningApprovalDateExist,
            new DateDetails("1", "2", "2023"),
            outlinePlanningApprovalDateExist,
            true,
            isGrantFundingForAllHomesExist,
            new DateDetails("1", "2", "2023"),
            planningSubmissionDateExist,
            false,
            null,
            null,
            isLandRegistryTitleNumberRegisteredExist);
    }
}
