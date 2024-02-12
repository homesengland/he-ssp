using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSiteUseDetailsCommand(
        SiteId SiteId,
        bool? IsPartOfStreetFrontInfill,
        bool? IsForTravellerPitchSite)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
