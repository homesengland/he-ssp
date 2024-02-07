using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideNumberOfGreenLightsCommand(SiteId SiteId, string? NumberOfGreenLights)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
