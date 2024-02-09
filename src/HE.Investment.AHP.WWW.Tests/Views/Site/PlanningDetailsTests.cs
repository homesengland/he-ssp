using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class PlanningDetailsTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/PlanningDetails.cshtml";

    [Theory]
    [InlineData(SitePlanningStatus.DetailedPlanningApprovalGranted, true, true, false, false, false, false, true, false, false)]
    [InlineData(SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps, true, true, false, false, false, false, true, true, false)]
    [InlineData(SitePlanningStatus.DetailedPlanningApplicationSubmitted, true, false, true, false, false, true, true, true, false)]
    [InlineData(SitePlanningStatus.OutlinePlanningApprovalGranted, true, false, false, true, false, true, true, true, false)]
    [InlineData(SitePlanningStatus.OutlinePlanningApplicationSubmitted, true, false, false, false, true, true, true, true, false)]
    [InlineData(SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice, false, false, false, false, false, true, false, false, true)]
    [InlineData(SitePlanningStatus.NoProgressOnPlanningApplication, false, false, false, false, false, true, false, false, true)]
    [InlineData(SitePlanningStatus.NoPlanningRequired, false, false, false, false, false, false, false, false, true)]
    public async Task ShouldDisplayView(
        SitePlanningStatus status,
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
        var model = CreateTestModel(status);
        var viewBag = new Dictionary<string, object> { { "SiteName", " some site name" } };

        // given & when
        var document = await Render(_viewPath, model, viewBag);

        // then
        document
            .HasPageHeader(viewBag["SiteName"].ToString())
            .HasInput(nameof(SiteModel.PlanningDetails.ReferenceNumber), value: model.ReferenceNumber, exist: referenceNumberExist)
            .HasDateInput(
                nameof(SitePlanningDetails.DetailedPlanningApprovalDate),
                day: model.DetailedPlanningApprovalDate?.Day,
                month: model.DetailedPlanningApprovalDate?.Month,
                year: model.DetailedPlanningApprovalDate?.Year,
                exist: detailedPlanningApprovalDateExist)
            .HasDateInput(
                nameof(SitePlanningDetails.ApplicationForDetailedPlanningSubmittedDate),
                day: model.ApplicationForDetailedPlanningSubmittedDate?.Day,
                month: model.ApplicationForDetailedPlanningSubmittedDate?.Month,
                year: model.ApplicationForDetailedPlanningSubmittedDate?.Year,
                exist: applicationForDetailedPlanningSubmittedDateExist)
            .HasDateInput(
                nameof(SitePlanningDetails.OutlinePlanningApprovalDate),
                day: model.OutlinePlanningApprovalDate?.Day,
                month: model.OutlinePlanningApprovalDate?.Month,
                year: model.OutlinePlanningApprovalDate?.Year,
                exist: outlinePlanningApprovalDateExist)
            .HasDateInput(
                nameof(SitePlanningDetails.PlanningSubmissionDate),
                day: model.PlanningSubmissionDate?.Day,
                month: model.PlanningSubmissionDate?.Month,
                year: model.PlanningSubmissionDate?.Year,
                exist: planningSubmissionDateExist)
            .HasDateInput(
                nameof(SitePlanningDetails.ExpectedPlanningApprovalDate),
                day: model.ExpectedPlanningApprovalDate?.Day,
                month: model.ExpectedPlanningApprovalDate?.Month,
                year: model.ExpectedPlanningApprovalDate?.Year,
                exist: expectedPlanningApprovalDateExist)
            .HasRadio(
                nameof(SitePlanningDetails.IsGrantFundingForAllHomesCoveredByApplication),
                new List<string> { "Yes", "No" },
                value: "True",
                exist: isGrantFundingForAllHomesExist)
            .HasTextAreaInput(nameof(SitePlanningDetails.RequiredFurtherSteps), value: model.RequiredFurtherSteps, exist: requiredFurtherStepsExist)
            .HasRadio(
                nameof(SitePlanningDetails.IsLandRegistryTitleNumberRegistered),
                new List<string> { "Yes", "No" },
                value: "False",
                exist: isLandRegistryTitleNumberRegisteredExist)
            .HasGdsSaveAndContinueButton()
            .HasGdsBackLink(false);
    }

    private static SitePlanningDetails CreateTestModel(SitePlanningStatus status)
    {
        return new SitePlanningDetails(
            status,
            "ref123",
            new DateDetails("1", "2", "2023"),
            "steps required",
            new DateDetails("1", "2", "2023"),
            new DateDetails("1", "2", "2023"),
            new DateDetails("1", "2", "2023"),
            true,
            new DateDetails("1", "2", "2023"),
            false,
            null,
            null);
    }
}
