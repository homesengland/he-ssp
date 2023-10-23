using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;
public record CreateFinancialDetailsCommand() : IRequest<OperationResult<CreateFinancialDetailsCommandResult>>;

public record CreateFinancialDetailsCommandResult(Guid FinancialDetailsId);
