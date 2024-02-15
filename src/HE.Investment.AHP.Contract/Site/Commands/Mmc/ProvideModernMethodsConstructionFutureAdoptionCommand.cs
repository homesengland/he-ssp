using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.Mmc;

public record ProvideModernMethodsConstructionFutureAdoptionCommand(
        SiteId SiteId,
        string? Plans,
        string? ExpectedImpact)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
