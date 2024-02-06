using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideBuildingForHealthyLifeCommand(SiteId SiteId, BuildingForHealthyLifeType BuildingForHealthyLife)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
