using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideLocalAuthorityCommand(
    SiteId SiteId,
    string? LocalAuthorityId,
    string? LocalAuthorityName) : IRequest<OperationResult>;
