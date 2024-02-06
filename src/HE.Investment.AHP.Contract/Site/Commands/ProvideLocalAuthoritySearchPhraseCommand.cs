using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideLocalAuthoritySearchPhraseCommand(SiteId SiteId, string? Phrase) : IRequest<OperationResult>;
