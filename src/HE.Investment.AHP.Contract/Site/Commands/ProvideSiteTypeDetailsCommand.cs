using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSiteTypeDetailsCommand(
        SiteId SiteId,
        SiteType? SiteType,
        bool? IsOnGreenBelt,
        bool? IsRegenerationSite)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
