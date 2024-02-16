using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSiteRuralClassificationCommand(
        SiteId SiteId,
        bool? IsWithinRuralSettlement,
        bool? IsRuralExceptionSite = null)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
