using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.Mmc;

public record ProvideSiteUsingModernMethodsOfConstructionCommand(
        SiteId SiteId,
        SiteUsingModernMethodsOfConstruction? UsingModernMethodsOfConstruction)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
