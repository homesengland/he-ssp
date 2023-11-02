using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideHomesTypesCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string[] HomesTypes, string OtherHomesTypes) : IRequest<OperationResult>;
