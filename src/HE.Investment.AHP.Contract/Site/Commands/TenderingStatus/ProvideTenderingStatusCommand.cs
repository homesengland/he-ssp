using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.TenderingStatus;

public record ProvideTenderingStatusCommand(
        SiteId SiteId,
        SiteTenderingStatus? TenderingStatus)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
