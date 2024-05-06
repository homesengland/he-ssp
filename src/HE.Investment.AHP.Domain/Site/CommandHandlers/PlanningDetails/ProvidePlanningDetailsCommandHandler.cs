using HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Factories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.PlanningDetails;

public class ProvidePlanningDetailsCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvidePlanningDetailsCommand>
{
    public ProvidePlanningDetailsCommandHandler(
        ISiteRepository siteRepository,
        IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvidePlanningDetailsCommand request, SiteEntity site)
    {
        var approvalDateInput = request.DetailedPlanningApprovalDate;
        var submittedDateInput = request.ApplicationForDetailedPlanningSubmittedDate;
        var expectedDateInput = request.ExpectedPlanningApprovalDate;
        var outlineDateInput = request.OutlinePlanningApprovalDate;
        var submissionDateInput = request.PlanningSubmissionDate;

        var operationResult = OperationResult.New();
        var referenceNumber = operationResult.AggregateNullable(() => ReferenceNumber.Create(request.ReferenceNumber));
        var approvalDate = operationResult.AggregateNullable(() =>
            DetailedPlanningApprovalDate.FromDateDetails(request.IsDetailedPlanningApprovalDateActive, approvalDateInput));
        var requiredFurtherSteps = operationResult.AggregateNullable(() => RequiredFurtherSteps.Create(request.RequiredFurtherSteps));
        var submittedDate = operationResult.AggregateNullable(() =>
            ApplicationForDetailedPlanningSubmittedDate.FromDateDetails(request.IsApplicationForDetailedPlanningSubmittedDateActive, submittedDateInput));
        var expectedDate = operationResult.AggregateNullable(() =>
            ExpectedPlanningApprovalDate.FromDateDetails(request.IsExpectedPlanningApprovalDateActive, expectedDateInput));
        var outlineDate = operationResult.AggregateNullable(() =>
            OutlinePlanningApprovalDate.FromDateDetails(request.IsOutlinePlanningApprovalDateActive, outlineDateInput));
        var submissionDate = operationResult.AggregateNullable(() =>
            PlanningSubmissionDate.FromDateDetails(request.IsPlanningSubmissionDateActive, submissionDateInput));
        var landRegistryDetails = operationResult.AggregateNullable(() =>
            LandRegistryDetails.WithIsRegistered(site.PlanningDetails.LandRegistryDetails, request.IsLandRegistryTitleNumberRegistered));
        operationResult.CheckErrors();

        var planningDetails = PlanningDetailsFactory.WithDetails(
            site.PlanningDetails,
            referenceNumber,
            approvalDate,
            requiredFurtherSteps,
            submittedDate,
            expectedDate,
            outlineDate,
            submissionDate,
            request.IsGrantFundingForAllHomesCoveredByApplication,
            landRegistryDetails);

        site.ProvidePlanningDetails(planningDetails);
    }
}
