using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideLocationCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string TypeOfLocation, string Coordinates, string LandregistryTitleNumber) : IRequest<OperationResult>;
