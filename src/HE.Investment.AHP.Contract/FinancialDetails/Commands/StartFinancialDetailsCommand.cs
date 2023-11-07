using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;
public record StartFinancialDetailsCommand(Guid ApplicationId, string ApplicationName) : IRequest<OperationResult>;
