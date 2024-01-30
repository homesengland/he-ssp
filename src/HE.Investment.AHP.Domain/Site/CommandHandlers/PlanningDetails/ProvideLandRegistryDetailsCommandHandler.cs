using HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Factories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.PlanningDetails;

public class ProvideLandRegistryDetailsCommandHandler : ProvidePlanningDetailsBaseCommandHandler<ProvideLandRegistryDetailsCommand>
{
    public ProvideLandRegistryDetailsCommandHandler(
        ISiteRepository siteRepository,
        IAccountUserContext accountUserContext,
        ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    protected override void Provide(ProvideLandRegistryDetailsCommand request, SiteEntity site)
    {
        var operationResult = OperationResult.New();
        var titleNumber = operationResult.AggregateNullable(() => new LandRegistryTitleNumber(request.LandRegistryTitleNumber));
        var landRegistryDetails = operationResult.AggregateNullable(() =>
            new LandRegistryDetails(site.PlanningDetails.LandRegistryDetails?.IsLandRegistryTitleNumberRegistered, titleNumber));
        operationResult.CheckErrors();

        var planningDetails = PlanningDetailsFactory.Create(
            site.PlanningDetails.PlanningStatus,
            site.PlanningDetails.ReferenceNumber,
            site.PlanningDetails.DetailedPlanningApprovalDate,
            site.PlanningDetails.RequiredFurtherSteps,
            site.PlanningDetails.ApplicationForDetailedPlanningSubmittedDate,
            site.PlanningDetails.ExpectedPlanningApprovalDate,
            site.PlanningDetails.OutlinePlanningApprovalDate,
            site.PlanningDetails.PlanningSubmissionDate,
            request.IsGrantFundingForAllHomes,
            landRegistryDetails);

        site.ProvidePlanningDetails(planningDetails);
    }
}
