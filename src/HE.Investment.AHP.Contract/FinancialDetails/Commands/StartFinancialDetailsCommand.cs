using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;
public record StartFinancialDetailsCommand(Guid FinancialSchemeId) : IRequest<OperationResult<StartFinancialDetailsCommandResult>>;

public record StartFinancialDetailsCommandResult(Guid FinancialDetailsId);
