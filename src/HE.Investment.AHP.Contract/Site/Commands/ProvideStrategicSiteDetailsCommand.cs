using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideStrategicSiteDetailsCommand(
        SiteId SiteId,
        bool? IsStrategicSite,
        string? StrategicSiteName)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
