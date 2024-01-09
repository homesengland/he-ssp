using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;

public record ProvideLocalAuthoritySearchPhraseCommand(string? Phrase) : IRequest<OperationResult>;
