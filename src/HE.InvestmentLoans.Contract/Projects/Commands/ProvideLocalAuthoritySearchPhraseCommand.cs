using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;

public record ProvideLocalAuthoritySearchPhraseCommand(string? Phrase) : IRequest<OperationResult>;
