using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;
public record StartFinancialDetailsCommand() : IRequest<OperationResult<StartFinancialDetailsCommandResult>>;

public record StartFinancialDetailsCommandResult(Guid FinancialDetailsId);
