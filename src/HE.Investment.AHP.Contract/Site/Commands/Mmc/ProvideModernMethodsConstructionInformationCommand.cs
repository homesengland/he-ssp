using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.Mmc;

public record ProvideModernMethodsConstructionInformationCommand(
        SiteId SiteId,
        string? Barriers,
        string? Impact)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
