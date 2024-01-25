using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSection106LocalAuthorityConfirmationCommand(SiteId SiteId, string? LocalAuthorityConfirmation) : IRequest<OperationResult>;
