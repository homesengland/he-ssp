using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideHomesTypesCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string[] HomesTypes, string OtherHomesTypes) : IRequest<OperationResult>;
