using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeHousingNeedsCommand(string ApplicationId, string TypeAndTenureJustification, string SchemeAndProposalJustification) : IRequest<OperationResult>, IUpdateSchemeCommand;
