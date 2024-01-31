using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;

public record ProvideLandRegistryDetailsCommand(
        SiteId SiteId,
        string? LandRegistryTitleNumber,
        bool? IsGrantFundingForAllHomesCoveredByTitleNumber)
    : IRequest<OperationResult>, IProvideSitePlanningDetailsCommand;
