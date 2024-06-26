using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Factories;

public static class PlanningDetailsFactory
{
    public static PlanningDetails CreateEmpty() => new EmptyPlanningDetails();

    public static PlanningDetails Create(PlanningDetails planningDetails, SitePlanningStatus? planningStatus) => Create(
        planningStatus,
        planningDetails.ReferenceNumber,
        planningDetails.DetailedPlanningApprovalDate,
        planningDetails.RequiredFurtherSteps,
        planningDetails.ApplicationForDetailedPlanningSubmittedDate,
        planningDetails.ExpectedPlanningApprovalDate,
        planningDetails.OutlinePlanningApprovalDate,
        planningDetails.PlanningSubmissionDate,
        planningDetails.IsGrantFundingForAllHomesCoveredByApplication,
        planningDetails.LandRegistryDetails);

    public static PlanningDetails Create(
        SitePlanningStatus? planningStatus,
        ReferenceNumber? referenceNumber = null,
        DetailedPlanningApprovalDate? detailedPlanningApprovalDate = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        ApplicationForDetailedPlanningSubmittedDate? applicationForDetailedPlanningSubmittedDate = null,
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        OutlinePlanningApprovalDate? outlinePlanningApprovalDate = null,
        PlanningSubmissionDate? planningSubmissionDate = null,
        bool? isGrantFundingForAllHomes = null,
        LandRegistryDetails? landRegistryDetails = null)
    {
        return planningStatus switch
        {
            SitePlanningStatus.DetailedPlanningApprovalGranted =>
                new DetailedPlanningApprovalGrantedPlanningDetails(referenceNumber, detailedPlanningApprovalDate, isGrantFundingForAllHomes),
            SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps =>
                new DetailedPlanningApprovalGrantedWithFurtherStepsPlanningDetails(
                    referenceNumber,
                    detailedPlanningApprovalDate,
                    requiredFurtherSteps,
                    isGrantFundingForAllHomes),
            SitePlanningStatus.DetailedPlanningApplicationSubmitted =>
                new DetailedPlanningApplicationSubmittedPlanningDetails(
                    referenceNumber,
                    requiredFurtherSteps,
                    applicationForDetailedPlanningSubmittedDate,
                    expectedPlanningApprovalDate,
                    isGrantFundingForAllHomes),
            SitePlanningStatus.OutlinePlanningApprovalGranted =>
                new OutlinePlanningApprovalGrantedPlanningDetails(
                    referenceNumber,
                    requiredFurtherSteps,
                    expectedPlanningApprovalDate,
                    outlinePlanningApprovalDate,
                    isGrantFundingForAllHomes),
            SitePlanningStatus.OutlinePlanningApplicationSubmitted =>
                new OutlinePlanningApplicationSubmittedPlanningDetails(
                    referenceNumber,
                    requiredFurtherSteps,
                    expectedPlanningApprovalDate,
                    isGrantFundingForAllHomes,
                    planningSubmissionDate),
            SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice =>
                new PlanningDiscussionsUnderwayWithThePlanningOfficePlanningDetails(expectedPlanningApprovalDate, landRegistryDetails),
            SitePlanningStatus.NoProgressOnPlanningApplication =>
                new NoProgressOnPlanningApplicationPlanningDetails(expectedPlanningApprovalDate, landRegistryDetails),
            SitePlanningStatus.NoPlanningRequired =>
                new NoPlanningRequiredPlanningDetails(landRegistryDetails),
            null => throw new DomainValidationException("PlanningStatus", "Select the planning status"),
            _ => throw new DomainValidationException("PlanningStatus", $"Planning status {planningStatus} is not supported."),
        };
    }

    public static PlanningDetails WithLandRegistryDetails(
        PlanningDetails planningDetails,
        LandRegistryDetails? landRegistryDetails = null)
    {
        return Create(
            planningDetails.PlanningStatus,
            planningDetails.ReferenceNumber,
            planningDetails.DetailedPlanningApprovalDate,
            planningDetails.RequiredFurtherSteps,
            planningDetails.ApplicationForDetailedPlanningSubmittedDate,
            planningDetails.ExpectedPlanningApprovalDate,
            planningDetails.OutlinePlanningApprovalDate,
            planningDetails.PlanningSubmissionDate,
            planningDetails.IsGrantFundingForAllHomesCoveredByApplication,
            landRegistryDetails);
    }

    public static PlanningDetails WithDetails(
        PlanningDetails planningDetails,
        ReferenceNumber? referenceNumber = null,
        DetailedPlanningApprovalDate? detailedPlanningApprovalDate = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        ApplicationForDetailedPlanningSubmittedDate? applicationForDetailedPlanningSubmittedDate = null,
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        OutlinePlanningApprovalDate? outlinePlanningApprovalDate = null,
        PlanningSubmissionDate? planningSubmissionDate = null,
        bool? isGrantFundingForAllHomesCoveredByApplication = null,
        LandRegistryDetails? landRegistryDetails = null)
    {
        return Create(
            planningDetails.PlanningStatus,
            referenceNumber,
            detailedPlanningApprovalDate,
            requiredFurtherSteps,
            applicationForDetailedPlanningSubmittedDate,
            expectedPlanningApprovalDate,
            outlinePlanningApprovalDate,
            planningSubmissionDate,
            isGrantFundingForAllHomesCoveredByApplication,
            landRegistryDetails);
    }
}
