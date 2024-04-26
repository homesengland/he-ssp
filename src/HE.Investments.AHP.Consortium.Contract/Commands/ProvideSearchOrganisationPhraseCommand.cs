using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record ProvideSearchOrganisationPhraseCommand(ConsortiumId ConsortiumId, string? Phrase) : IRequest<OperationResult>;
