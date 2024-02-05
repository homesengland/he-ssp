using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.TenderingStatus;

public record ProvideIsIntentionToWorkWithSmeCommand(
        SiteId SiteId,
        bool? IsIntentionToWorkWithSme)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
