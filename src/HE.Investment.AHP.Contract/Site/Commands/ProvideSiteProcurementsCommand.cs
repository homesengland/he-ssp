using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSiteProcurementsCommand(
        SiteId SiteId,
        IList<SiteProcurement> Procurements)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
