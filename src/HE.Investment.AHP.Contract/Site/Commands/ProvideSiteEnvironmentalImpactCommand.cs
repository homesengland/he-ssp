using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSiteEnvironmentalImpactCommand(SiteId SiteId, string? EnvironmentalImpact)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
