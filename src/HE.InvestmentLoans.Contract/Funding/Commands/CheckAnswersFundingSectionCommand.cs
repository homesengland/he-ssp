using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Funding.Commands;
public record CheckAnswersFundingSectionCommand(LoanApplicationId LoanApplicationId, string YesNoAnswer) : IRequest<OperationResult>;
