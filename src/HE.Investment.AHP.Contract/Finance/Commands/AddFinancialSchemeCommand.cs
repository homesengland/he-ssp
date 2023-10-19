using HE.Investment.AHP.Contract.Finance.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Contract.Finance.Commands;
public record AddFinancialSchemeCommand() : IRequest<OperationResult<FinancialSchemeId>>;
