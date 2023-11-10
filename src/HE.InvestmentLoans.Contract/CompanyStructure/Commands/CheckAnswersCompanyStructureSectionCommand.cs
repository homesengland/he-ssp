using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record CheckAnswersCompanyStructureSectionCommand(LoanApplicationId LoanApplicationId, string YesNoAnswer) : IRequest<OperationResult>;
