using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Factories;

public static class PlanningDetailsFactory
{
    public static PlanningDetails CreateEmpty() => new EmptyPlanningDetails();

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
                new DetailedPlanningApprovalGrantedPlanningDetails(referenceNumber, detailedPlanningApprovalDate),
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
            null => throw new DomainValidationException("PlanningStatus", "Please select value"),
            _ => throw new DomainValidationException("PlanningStatus", $"Value {planningStatus} is not supported."),
        };
    }
}
