using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideTravellerPitchSiteTypeCommand(
        SiteId SiteId,
        TravellerPitchSiteType TravellerPitchSiteType)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
