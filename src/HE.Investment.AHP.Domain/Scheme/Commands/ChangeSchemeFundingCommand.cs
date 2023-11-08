using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeFundingCommand(string ApplicationId, string? RequiredFunding, string? HousesToDeliver) : IRequest<OperationResult>, IUpdateSchemeCommand;
