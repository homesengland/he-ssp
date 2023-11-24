using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Funding.Commands;
public record CheckAnswersFundingSectionCommand(LoanApplicationId LoanApplicationId, string YesNoAnswer) : IRequest<OperationResult>;
