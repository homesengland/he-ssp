using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSitePlanningStatusCommand(SiteId SiteId, SitePlanningStatus SitePlanningStatus) : IRequest<OperationResult>;
