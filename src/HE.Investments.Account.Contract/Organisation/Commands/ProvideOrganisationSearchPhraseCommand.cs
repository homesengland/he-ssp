using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Commands;

public record ProvideOrganisationSearchPhraseCommand(string? Name) : IRequest<OperationResult>;
