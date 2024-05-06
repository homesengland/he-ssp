using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideLocalAuthorityCommand(
    SiteId SiteId,
    string? LocalAuthorityCode,
    string? LocalAuthorityName,
    string? Response) : IRequest<OperationResult>;
